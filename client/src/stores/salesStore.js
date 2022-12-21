import {onBeforeMount, reactive} from 'vue'
import {defineStore} from 'pinia'
import axios from 'axios'
import _ from 'lodash'

const useSalesStore = defineStore('salesStore', () => {
    const pageInfo = reactive({
        currentPage: 1,
        totalPages: 15,
        pageSize: 10,
        hasPreviousPage: false,
        hasNextPage: false,
    })

    const filter = reactive({
        sellerName: {
            value: null,
            type: String
        },
        sellerSurname: {
            value: null,
            type: String
        },
        minDate: {
            value: null,
            type: String
        },
        maxDate: {
            value: null,
            type: String
        },
        currentPage: 1,
        columnToSort: 'date',
        sortType: 'desc'
    })

    const sales = reactive({
        items: [],
        totalCount: 0,
        minDate: null,
        maxDate: null
    })

    onBeforeMount(async () => {
        await debouncedFetchSales()
    })

    const debouncedFetchSales = _.debounce(fetchSales, 200)

    async function fetchSales() {
        let response = await axios.get('api/sales', {
            params: filter
        })

        sales.items = response.data.items
        sales.totalCount = response.data.totalCount
        sales.minDate = new Date(response.data.dateMin).toISOString().split('T')[0]
        sales.maxDate = new Date(response.data.dateMax).toISOString().split('T')[0]

        pageInfo.currentPage = response.data.currentPage
        pageInfo.totalPages = response.data.totalPages
        pageInfo.pageSize = response.data.pageSize
        pageInfo.hasPreviousPage = response.data.hasPreviousPage
        pageInfo.hasNextPage = response.data.hasNextPage
    }

    return {
        debouncedFetchSales,
        pageInfo,
        filter,
        sales
    }
})

export default useSalesStore