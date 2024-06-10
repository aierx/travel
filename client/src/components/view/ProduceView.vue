<script setup>
import {onMounted} from "vue";
import {useUserInfoStore} from "../../stores/user.js";
const user = useUserInfoStore()
onMounted(()=>{
    user.page = "product"
})
</script>

<template>
    <router-view/>
</template>