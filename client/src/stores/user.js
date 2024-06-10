// stores/counter.js
import {defineStore} from 'pinia';

const useUserInfoStore = defineStore('userInfo', {
    state: () => ({
        isLogin: false,
        isAdmin: false,
        id: null,
        name: "",
        userNo: "",
        aliasName: "",
        sex: "",
        birthDate: "",
        phone: "",
        email: "",
        address: "",
        balance: null,
        passwd: "",
        newPasswd: null,
        page: "product",
        secondPage:"user",
        attr1:"",
        attr2:"",
    }),
    persist: {
        enabled: true,
        strategies: [{key: 'user', storage: localStorage}, {storage: localStorage, paths: ['id', 'aliasName']}]
    },
    actions: {}
});


export {useUserInfoStore}