<script setup>
import useSellersStore from '@/stores/sellersStore'
import {reactive} from 'vue'
import {FontAwesomeIcon} from '@fortawesome/vue-fontawesome'

const sellersStore = useSellersStore()

const filter = reactive({
  name: '',
  surname: '',
  fname: '',
  phoneNumber: {
    value: null,
    type: Number
  },
  includeNotActive: false
})

async function onApplyFilter() {
  sellersStore.filter.name = filter.name
  sellersStore.filter.surname = filter.surname
  sellersStore.filter.fname = filter.fname
  sellersStore.filter.phoneNumber = filter.phoneNumber
  sellersStore.filter.includeNotActive = filter.includeNotActive

  await sellersStore.debouncedFetchSellers()
}

async function onResetFilter() {
  filter.name = ''
  filter.surname = ''
  filter.fname = ''
  filter.phoneNumber = null
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
          <label for="floatingInput">Имя</label>
        </div>
      </div>

      <div role="group">
        <div class="form-floating">
          <input
              v-model="filter.surname"
              type="text"
              class="form-control my-2"
              placeholder="surname">
          <label for="floatingInput">Фамилия</label>
        </div>
      </div>

      <div role="group">
        <div class="form-floating">
          <input
              v-model="filter.fname"
              type="text"
              class="form-control my-2"
              placeholder="fname">
          <label for="floatingInput">Отчество</label>
        </div>
      </div>

      <div role="group">
        <div class="form-floating">
          <input
              v-model="filter.phoneNumber"
              type="number"
              class="form-control my-2"
              placeholder="phoneNumber">
          <label for="floatingInput">Номер телефона</label>
        </div>
      </div>

      <div role="group">
        <b-form-checkbox
            v-model="filter.includeNotActive"
            :value="true"
            :unchecked-value="false"
            switch="">
          Показывать отстранённых
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