import {createApp} from 'vue'
import ElementPlus, {ElMessage} from 'element-plus'
import VForm3 from 'vform3-builds'
import {createPinia} from 'pinia'
import 'element-plus/dist/index.css'
import 'vform3-builds/dist/designer.style.css'
import App from './App.vue'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'
import router from "./router/router.js";
import axios from 'axios'
import piniaPersist from 'pinia-plugin-persistedstate'

const app = createApp(App)
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
}
const $axios = axios.create({baseURL: "http://localhost:5101", withCredentials: true,})
$axios.interceptors.response.use(function (e) {
    if (e.data.code === 401) {
        ElMessage.error('请登入')
        setTimeout(() => {
            router.replace("/login")
        })

    }
    if (e.data.code === 403) {
        ElMessage.error('没有权限')
        setTimeout(() => {
            router.replace("/login")
        })
    }
    return e
})
const pinia = createPinia()
pinia.use(piniaPersist)
app.provide('$axios', $axios)
app.use(pinia)
app.use(ElementPlus)
app.use(router)
app.use(VForm3)
app.mount('#app')