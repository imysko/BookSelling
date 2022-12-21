import {onBeforeMount, reactive} from 'vue'
import {defineStore} from 'pinia'
import axios from 'axios'
import _ from 'lodash'

const useSellersStore = defineStore('', () => {
    const pageInfo = reactive({
        currentPage: 1,
        totalPages: 15,
        pageSize: 10,
        hasPreviousPage: false,
        hasNextPage: false,
    })

    const filter = reactive({
        name: {
            value: null,
            type: String
        },
        surname: {
            value: null,
            type: String
        },
        fname: {
            value: null,
            type: String
        },
        phoneNumber: {
            value: null,
            type: Number
        },
        currentPage: 1,
        columnToSort: 'name',
        sortType: 'asc'
    })

    const sellers = reactive({
        items: [],
        totalCount: 0,
    })

    onBeforeMount(async () => {
        await debouncedFetchSellers()
    })

    const debouncedFetchSellers = _.debounce(fetchSellers, 100)

    async function fetchSellers() {
        let response = await axios.get('api/sellers', {
            params: filter
        })

        sellers.items = response.data.items
        sellers.totalCount = response.data.totalCount

        pageInfo.currentPage = response.data.currentPage
        pageInfo.totalPages = response.data.totalPages
        pageInfo.pageSize = response.data.pageSize
        pageInfo.hasPreviousPage = response.data.hasPreviousPage
        pageInfo.hasNextPage = response.data.hasNextPage
    }

    return {
        debouncedFetchSellers,
        pageInfo,
        filter,
        sellers
    }
})

export default useSellersStore