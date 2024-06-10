import { createRouter, createWebHistory } from 'vue-router'
import IndexView from "../views/IndexView.vue";
import LoginView from "../views/LoginView.vue";
import RegisterView from "../views/RegisterView.vue";
import EditTable from "../views/EditTable.vue"

import ManagerView from "../components/view/ManagerView.vue";
import ProduceView from "../components/view/ProduceView.vue";
import PersonalCenterView from "../components/view/PersonalCenterView.vue";

import UserManager from "../components/manager/UserManager.vue";
import HotelManager from "../components/manager/HotelManager.vue";
import SceneryManager from "../components/manager/SceneryManager.vue";
import OrderManager from "../components/manager/OrderManager.vue";

import ProductDetail from "../components/detail/ProductDetail.vue";
import ProductList from "../components/detail/ProductList.vue";

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: 'home',
            component: IndexView,
            redirect: "/product/list",
            children:[
                {
                    path:"admin",
                    component:ManagerView,
                    redirect:"/admin/user",
                    children:[
                        {
                            path:"user",
                            component:UserManager
                        },
                        {
                            path:"scenery",
                            component:SceneryManager
                        },
                        {
                            path:"hotel",
                            component:HotelManager
                        },
                        {
                            path:"order",
                            component:OrderManager
                        }
                    ]
                },
                {
                    path:"product",
                    component:ProduceView,
                    redirect:"/product/list",
                    children:[
                        {
                            path: "list",
                            component: ProductList,
                        },
                        {
                            path:"detail/:id",
                            component: ProductDetail
                        }
                    ]
                },
                {
                    path:"user",
                    component:PersonalCenterView
                },
            ]
        },
        {
            path: '/login',
            name: 'login',
            component:LoginView
        },
        {
            path: '/register',
            name: 'register',
            component:RegisterView
        },
        {
            path: '/table',
            name: 'table',
            component:EditTable
        },
    ]
})

export default router