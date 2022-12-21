<script setup>
import useAuthenticationStore from '@/stores/authenticationStore'
import {FontAwesomeIcon} from '@fortawesome/vue-fontawesome'

const authStore = useAuthenticationStore()

const emit = defineEmits(['active-seller-click', 'edit-seller-click'])

const props = defineProps({
  seller: Object
})

function onActiveClick(seller, active) {
  emit("active-seller-click", seller, active)
}

function onEditClick(seller) {
  emit('edit-seller-click', seller)
}
</script>

<template>
  <b-card
      no-body
      :title="`${props.seller.name} ${props.seller.surname}`"
      :border-variant="props.seller.active ? 'grey' : 'secondary'"
      class="m-2">
    <b-card-body>
      <b-card-title>
        {{ props.seller.name }} {{ props.seller.surname }}
      </b-card-title>

      <b-card-subtitle>
        {{ props.seller.fname }}
      </b-card-subtitle>

      <b-card-text>
        <b>Телефон:</b> {{ props.seller.phoneNumber }} <br />
      </b-card-text>
    </b-card-body>

    <b-card-footer>
      <b-card-group class="d-flex">
        <b-button
            v-if="props.seller.active"
            variant="outline-success"
            pill
            @click="onActiveClick(props.seller, false)">
          Активный
        </b-button>
        <b-button
            v-else
            variant="outline-secondary"
            pill
            @click="onActiveClick(props.seller, true)">
          Отстранён
        </b-button>

        <b-button
            pill
            variant="outline-warning"
            class="ms-2"
            @click="onEditClick({ ...props.seller })">
          <FontAwesomeIcon icon="fas fa-pen"/>
        </b-button>
      </b-card-group>
    </b-card-footer>
  </b-card>
</template>

<style scoped>
</style>