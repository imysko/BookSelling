<script setup>
import {computed, ref} from 'vue'
import _ from 'lodash'
import useAuthenticationStore from '@/stores/authenticationStore'
import useSalesStore from '@/stores/salesStore'
import axios from "axios";

const props = defineProps({
  sale: Object,
  sellers: Array
})

const authenticationStore = useAuthenticationStore()
const salesStore = useSalesStore()

const sellerId = ref(props.sellers.length ? props.sellers[0].id : Number)

const formatDate = computed(() => {
  let d = new Date(props.sale.date),
      month = "" + (d.getMonth() + 1),
      day = "" + d.getDate(),
      year = d.getFullYear()

  if (month.length < 2) month = "0" + month;
  if (day.length < 2) day = "0" + day;
  return [day, month, year].join(".");
})

const amount = computed(() => {
  return _.sum(props.sale.salesBooks.map(sb => sb.book.price * sb.soldCount))
})

async function onConfirm() {
  await axios.put(`api/sales/${props.sale.id}?sellerId=${sellerId.value}`)
  await salesStore.debouncedFetchSales()
}
</script>

<template>
  <b-card
      no-body
      class="m-2">
    <b-card-body>
      <div v-if="props.sale.sellerId !== null">
        <b-card-title>
          {{ props.sale.seller.name }} {{ props.sale.seller.surname }}
        </b-card-title>

        <b-card-subtitle>
          {{ formatDate }}
        </b-card-subtitle>
      </div>
      <b-card-title v-else class="d-flex justify-content-sm-between">
        <div>
          Продажа не подтверждена
        </div>
        <div v-if="authenticationStore.canAction('admin')" class="d-flex flex-row">
          <select v-model="sellerId" required class="form-select me-4">
            <option v-for="seller in props.sellers" :value="seller.id">{{ seller.name }} {{ seller.surname }}</option>
          </select>

          <b-button pill variant="outline-success" @click="onConfirm">Подтвердить</b-button>
        </div>
      </b-card-title>
    </b-card-body>

    <b-list-group flush>
      <b-list-group-item class="d-flex justify-content-sm-between" v-for="saleBook in props.sale.salesBooks">
        <b-card-text>
          <b>Книга: </b>{{ saleBook.book.name }}
        </b-card-text>
        <b-card-text>
          <b>Продано: </b>{{ saleBook.soldCount }}<br/>
          <b>Цена: </b>{{ saleBook.book.price }} ₽
        </b-card-text>
      </b-list-group-item>
    </b-list-group>

    <b-card-footer class="d-flex justify-content-end">
      <h3>Итог: {{ amount }} ₽</h3>
    </b-card-footer>
  </b-card>
</template>

<style scoped>
</style>