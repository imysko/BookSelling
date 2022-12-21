import {onBeforeMount, reactive} from 'vue'
import {defineStore} from 'pinia'
import axios from 'axios'
import _ from 'lodash'

const useBooksStore = defineStore('booksStore', () => {
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
        author: {
            value: null,
            type: String
        },
        includeNotActive: false,
        yearMin: {
            value: null,
            type: Number
        },
        yearMax: {
            value: null,
            type: Number
        },
        priceMin: {
            value: null,
            type: Number
        },
        priceMax: {
            value: null,
            type: Number
        },
        genries: [],
        currentPage: 1,
        columnToSort: 'name',
        sortType: 'asc'
    })

    const books = reactive({
        items: [],
        totalCount: 0,
        yearMin: 0,
        yearMax: Number.MAX_SAFE_INTEGER,
        priceMin: 0,
        priceMax: Number.MAX_SAFE_INTEGER
    })

    const genries = reactive({
        items: [],
        totalCount: 0
    })

    onBeforeMount(async () => {
        await debouncedFetchBooks()
    })

    const debouncedFetchBooks = _.debounce(fetchBooks, 100)

    async function fetchBooks() {
        let response = await axios.get('api/books', {
            params: filter
        })

        books.items = response.data.items
        books.totalCount = response.data.totalCount
        books.yearMin = response.data.yearMin
        books.yearMax = response.data.yearMax
        books.priceMin = response.data.priceMin
        books.priceMax = response.data.priceMax

        pageInfo.currentPage = response.data.currentPage
        pageInfo.totalPages = response.data.totalPages
        pageInfo.pageSize = response.data.pageSize
        pageInfo.hasPreviousPage = response.data.hasPreviousPage
        pageInfo.hasNextPage = response.data.hasNextPage

        response = await axios.get('api/genries')

        genries.items = response.data.map(g => g.name)
        genries.totalCount = response.data.length
    }

    return {
        debouncedFetchBooks,
        pageInfo,
        filter,
        genries,
        books
    }
})

export default useBooksStore