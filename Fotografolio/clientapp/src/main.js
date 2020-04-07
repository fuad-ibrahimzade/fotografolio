//import Vue from 'vue'
//import App from '@/App.vue'

//Vue.config.productionTip = false

//new Vue({
//  render: h => h(App),
//}).$mount('#app')

//import '@/bootstrap'
import Vue from 'vue'
import VueRouter from 'vue-router'
import EasySlider from 'vue-easy-slider'

Vue.config.productionTip = false

try {
    window.$ = window.jQuery = require('jquery');

    require('bootstrap');
} catch (e) {
    var errormessage = e.message
    errormessage = errormessage+" pox"
}
window.axios = require('axios');
window.axios.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';
let token = document.head.querySelector('meta[name="csrf-token"]');
if (token) {
    window.axios.defaults.headers.common['X-CSRF-TOKEN'] = token.content;
} else {
    console.log('CSRF token not found');
}

Vue.use(VueRouter)
Vue.use(EasySlider)

import App from '@/components/App'
import HomeIndexUrl from '@/components/home/HomeIndexUrl.vue';
import GalleryUrl from '@/components/home/GalleryUrl.vue';
import AboutUrl from '@/components/home/AboutUrl.vue';
import ContactUrl from '@/components/home/ContactUrl.vue';

// import auth from '@/components/middleware/auth.vue';
import LoginUrl from '@/components/auth/LoginUrl.vue';
import DashboardIndexUrl from '@/components/dashboard/DashboardIndexUrl.vue';
import SlideUrl from '@/components/dashboard/SlideUrl.vue';
import AboutEditUrl from '@/components/dashboard/AboutEditUrl.vue';
import LogoUrl from '@/components/dashboard/LogoUrl.vue';
import LinkUrl from '@/components/dashboard/LinkUrl.vue';

import Welcome from '@/components/Welcome'
import Page from '@/components/Page'
import UsersIndex from '@/components/UsersIndex';

const router = new VueRouter({
    mode: 'history',
    routes: [
        {
            path: '/',
            name: 'home.index',
            component: HomeIndexUrl,
            meta: {
                title: 'Home Page',
            },
            props: { title: "Home Page" }
        },
        {
            path: '/about',
            name: 'about',
            component: AboutUrl,
        },
        {
            path: '/contact',
            name: 'contact',
            component: ContactUrl,
        },
        {
            path: '/gallery/:galleryname',
            name: 'gallery',
            component: GalleryUrl,
        },
        {
            path: '/login',
            name: 'login',
            component: LoginUrl,
        },
        {
            path: '/dashboard',
            name: 'dashboard',
            component: DashboardIndexUrl,
        },
        {
            path: '/slide',
            name: 'slide',
            component: SlideUrl,
        },
        {
            path: '/about/edit',
            name: 'about-edit',
            component: AboutEditUrl,
        },
        {
            path: '/logo',
            name: 'logo',
            component: LogoUrl,
        },
        {
            path: '/link',
            name: 'link',
            component: LinkUrl,
        },
        {
            path: '/spa-page',
            name: 'page',
            component: Page,
            props: {
                title: "This is the SPA Second Page",
                author: {
                    name: "Fisayo Afolayan",
                    role: "Software Engineer",
                    code: "Always keep it clean"
                }
            }
        },
        {
            path: '/users',
            name: 'users.index',
            component: UsersIndex,
        },
        { path: '*', redirect: '/' }
    ],
})
const app = new Vue({
    el: '#app',
    components: { App },
    render: h => h(App),
    router,
    data() {
        return {
            link: null,
            logo: null,
            galleries: null,
        };
    },
    computed: {
        csrf_token() {
            let token = document.head.querySelector('meta[name="csrf-token"]')
            return token.content
        },
        isLoggedIn() {
            // $("meta[name=login-status]").attr('content')     pox cixdi yuklemir arada
            return document.head.querySelector("meta[name=login-status]").content
        },
        api_token() {
            let token = document.head.querySelector('meta[name="api-token"]')
            return token.content
        },
    },
    mounted() {
        //console.log("csrfffffffffffffffff", this.csrf_token)
    }
});
// mounted () {
//     let root = this.myprop || '/home'
//     this.$router.push({ path: root });
// }
// router.replace('/');