<script setup>
import useBooksStore from '@/stores/booksStore'
import {computed, reactive, watch} from "vue";
import {FontAwesomeIcon} from "@fortawesome/vue-fontawesome";

const booksStore = useBooksStore()

const filter = reactive({
  name: '',
  author: '',
  yearMin: 0,
  yearMax: 0,
  priceMin: 0,
  priceMax: 0,
  genriesAll: false,
  genries: [],
  includeNotActive: false
})

const ranges = reactive({
  yearMin: 0,
  yearMax: Number.MAX_SAFE_INTEGER,
  priceMin: 0,
  priceMax: Number.MAX_SAFE_INTEGER
})

const computedRanges = computed(() => {
  ranges.yearMin = booksStore.books.yearMin
  ranges.yearMax = booksStore.books.yearMax
  ranges.priceMin = booksStore.books.priceMin
  ranges.priceMax = booksStore.books.priceMax

  filter.yearMin = ranges.yearMin
  filter.yearMax = ranges.yearMax
  filter.priceMin = ranges.priceMin
  filter.priceMax = ranges.priceMax

  return ranges
})

async function onApplyFilter() {
  booksStore.filter.name = filter.name
  booksStore.filter.author = filter.author
  booksStore.filter.yearMin = filter.yearMin
  booksStore.filter.yearMax = filter.yearMax
  booksStore.filter.priceMin = filter.priceMin
  booksStore.filter.priceMax = filter.priceMax
  booksStore.filter.genries = filter.genries
  booksStore.filter.includeNotActive = filter.includeNotActive

  booksStore.filter.currentPage = 1

  await booksStore.debouncedFetchBooks()
}

function onSelectGenries() {
  if (filter.genriesAll) {
    filter.genries = booksStore.genries.items
  }
  else {
    filter.genries = []
  }
}

async function onResetFilter() {
  filter.name = ''
  filter.author = ''
  filter.yearMin = ranges.yearMin
  filter.yearMax = ranges.yearMax
  filter.priceMin = ranges.priceMin
  filter.priceMax = ranges.priceMax
  filter.genriesAll = false
  filter.genries = []
  filter.includeNotActive = false

  await onApplyFilter()
}
</script>

<template>
  <b-card
    title="Фильтр">
    <b-row class="my-1">
      <div role="group">
        <div class="form-floating">
          <input
              v-model="filter.name"
              type="text"
              class="form-control my-2"
              placeholder="name">
          <label for="floatingInput">Наименование</label>
        </div>
      </div>

      <div role="group">
        <div class="form-floating">
          <input
              v-model="filter.author"
              type="text"
              class="form-control my-2"
              placeholder="author">
          <label for="floatingInput">Автор</label>
        </div>
      </div>

      <div role="group">
        <label>Начальный год: {{ filter.yearMin }}</label>
        <b-form-input
            v-model="filter.yearMin"
            class="multi-range"
            type="range"
            :min="computedRanges.yearMin"
            :max="filter.yearMax"/>
      </div>

      <div role="group">
        <label>Конченый год: {{ filter.yearMax }}</label>
        <b-form-input
            v-model="filter.yearMax"
            class="multi-range"
            type="range"
            :min="filter.yearMin"
            :max="computedRanges.yearMax"/>
      </div>

      <div role="group">
        <label>Минимальная цена: {{ filter.priceMin }}</label>
        <b-form-input
            v-model="filter.priceMin"
            class="multi-range"
            type="range"
            :min="computedRanges.priceMin"
            :max="filter.priceMax"/>
      </div>

      <div role="group">
        <label>Максимальная цена: {{ filter.priceMax }}</label>
        <b-form-input
            v-model="filter.priceMax"
            class="multi-range"
            type="range"
            :min="filter.priceMin"
            :max="computedRanges.priceMax"/>
      </div>

      <b-form-group>
        <b-button pill v-b-toggle.collapse variant="outline-warning" class="m-1">Жанры</b-button>
        <b-collapse id="collapse">
          <b-form-checkbox
              v-model="filter.genriesAll"
              @change="onSelectGenries">
            Все
          </b-form-checkbox>

          <b-form-checkbox-group
              v-model="filter.genries"
              :options="booksStore.genries.items"
              stacked>
          </b-form-checkbox-group>
        </b-collapse>
      </b-form-group>

      <div role="group">
        <b-form-checkbox
            v-model="filter.includeNotActive"
            :value="true"
            :unchecked-value="false"
            switch="">
          Показывать недоступные
        </b-form-checkbox>
      </div>

      <div role="group" class="d-flex justify-content-around mt-3">
        <b-button pill variant="outline-danger" @click="onResetFilter">
          <font-awesome-icon icon="fas fa-rotate-left" size="md" />
        </b-button>
        <b-button pill variant="outline-warning" @click="onApplyFilter">Применить</b-button>
      </div>
    </b-row>
  </b-card>
</template>

<style scoped>
</style>