<script setup>
import {computed} from 'vue'
import _ from 'lodash'

const props = defineProps({
  sale: Object
})

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
</script>

<template>
  <b-card
      no-body
      :title="`${props.sale.seller.name} ${props.sale.seller.surname}`"
      class="m-2">
    <b-card-body>
      <b-card-title>
        {{ props.sale.seller.name }} {{ props.sale.seller.surname }}
      </b-card-title>

      <b-card-subtitle>
        {{ formatDate }}
      </b-card-subtitle>
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