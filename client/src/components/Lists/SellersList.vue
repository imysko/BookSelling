<script setup>
import {ref} from 'vue'
import SellerChangeStatusModal from '@/components/Modals/SellerChangeStatusModal.vue'
import ChangeSellerModal from '@/components/Modals/ChangeSellerModal.vue'
import SellerCard from '@/components/Cards/SellerCard.vue'
import useSellersStore from '@/stores/sellersStore'

const sellersStore = useSellersStore()

const changeStatusModalRef = ref()
const changeSellerModalRef = ref()

function onActiveClick(seller, status) {
  changeStatusModalRef.value.handSeller(seller)
  changeStatusModalRef.value.show()
}

function onEditClick(seller) {
  changeSellerModalRef.value.handSeller(seller)
  changeSellerModalRef.value.show()
}

async function changeStatus(id, status) {
  sellersStore.sellerStatus.id = id
  sellersStore.sellerStatus.active = status

  await sellersStore.changeStatus()
  await sellersStore.debouncedFetchSellers()
}

async function editSeller(seller) {
  sellersStore.seller.id = seller.id
  sellersStore.seller.name = seller.name
  sellersStore.seller.surname = seller.surname
  sellersStore.seller.fname = seller.fname
  sellersStore.seller.phoneNumber = seller.phoneNumber

  await sellersStore.changeSeller()
  await sellersStore.debouncedFetchSellers()
}
</script>

<template>
  <SellerChangeStatusModal @active="changeStatus" ref="changeStatusModalRef"/>
  <ChangeSellerModal @seller-edit="editSeller" ref="changeSellerModalRef"/>

  <div v-if="sellersStore.sellers.items.length">
    <SellerCard
        @active-seller-click="onActiveClick"
        @edit-seller-click="onEditClick"
        v-for="seller in sellersStore.sellers.items"
        :seller="seller"/>

    <b-pagination
        pills
        v-model="sellersStore.filter.currentPage"
        v-on:click="sellersStore.debouncedFetchSellers"
        :total-rows="sellersStore.pageInfo.totalPages * sellersStore.pageInfo.pageSize"
        :per-page="sellersStore.pageInfo.pageSize"
        first-number
        last-number
        align="center">
    </b-pagination>
  </div>
  <div v-else class="d-flex justify-content-center">
    Ничего не найдено!
  </div>
</template>

<style scoped>
</style>