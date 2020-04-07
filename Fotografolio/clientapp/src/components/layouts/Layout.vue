<template>
    <div>

        <!-- Fonts -->
        <link rel="dns-prefetch" href="https://fonts.gstatic.com">
        <link href="https://fonts.googleapis.com/css?family=Nunito" rel="stylesheet" type="text/css">
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

        <div id="">
            <nav class="navbar navbar-expand-md navbar-light navbar-laravel">
                <div class="container">
                    <router-link class="navbar-brand" to="/">
                        Photographer
                    </router-link>
                    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                        <span class="navbar-toggler-icon"></span>
                    </button>

                    <div class="collapse navbar-collapse" id="navbarSupportedContent">
                        <!-- Left Side Of Navbar -->
                        <ul class="navbar-nav mr-auto">

                        </ul>

                        <!-- Right Side Of Navbar -->
                        <ul class="navbar-nav ml-auto">
                            <!-- Authentication Links -->
                            <li class="nav-item dropdown" v-if="isLoggedIn">
                                <router-link to="/dashboard" class="nav-link float-left mr-2">Portfolio</router-link>
                                <router-link to="/slide" class="nav-link float-left mr-2">Slide</router-link>
                                <router-link to="/about/edit" class="nav-link float-left mr-2">About</router-link>
                                <router-link to="/logo" class="nav-link float-left mr-2">Logo</router-link>
                                <router-link to="/link" class="nav-link float-left mr-2">Link</router-link>
                                <a href="/logout" class="nav-link float-left mr-2 bg-transparent border-0 pt-4 mt-1"
                                   onclick="event.preventDefault();
                                                     document.getElementById('logout-formb').click();">
                                    Logout
                                </a>
                                <form id="logout-form" action="/logout" method="POST" @submit.stop.prevent="logout($event)" style="display: none;">
                                    <input type="hidden" name="__RequestVerificationToken" :value="csrf_token">
                                    <button id="logout-formb" type="submit"></button>
                                </form>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>

            <main class="py-4">
                <!--<div class="loading" v-if="loading">-->
                    <!--Loading...-->
                <!--</div>-->
                <!--<div class="d-flex align-items-center loading" v-if="loading">-->
                    <!--<strong>Loading...</strong>-->
                    <!--<div class="spinner-border ml-auto" role="status" aria-hidden="true"></div>-->
                <!--</div>-->
                <div class="d-flex justify-content-center loading" v-if="loading">
                    <span >Loading...</span>&nbsp;&nbsp;
                    <div class="spinner-border" role="status">

                    </div>
                </div>
                <keep-alive>
                    <component v-bind:is="currentLinkComponent" ref="childComponent"></component>
                </keep-alive>
            </main>
        </div>
        <div id="scripts"></div>
    </div>
</template>

<script>
    export default {
        name: "Layout.vue",
        data() {
            return {
                loading: false,
                isLoggedIn: this.$root.isLoggedIn,
                csrf_token:this.$root.csrf_token,
                error: null,
                currentLinkComponent: null,
            };
        },
        created(){
            var isLoggedIn = $("meta[name=login-status]").attr('content');
            var currentUrl = window.location.pathname;
            if(!isLoggedIn && currentUrl!='/login'){
                this.$router.push({path:'/login'})
            }
        },
        mounted(){
            let script1 = document.createElement('script')
            script1.setAttribute('src', 'js/app.js')
            script1.defer=true

            if(this.link || this.logo || this.galleries){
                this.$root.link=null;
                this.$root.logo=null;
                this.$root.galleries=null;
            }
        },
        computed: {
            link(){ return this.$root.link},
            logo(){ return this.$root.logo},
            galleries(){ return this.$root.galleries},
        },
        methods: {
            setData(err, {data: galleries, link, logo}) {
                if (err) {
                    this.error = err.toString();
                } else {
                    this.galleries = galleries;
                    this.link = link;
                    this.logo = logo;
                    // console.log(this.galleries)
                }
            },
            swapComponent: function(component)
            {
                this.currentLinkComponent= component;
            },
            startGettingData(){
                this.loading=true;
                axios
                    .get('/api/homedata')
                    .then(response => {
                        this.loading=false;
                        this.galleries = response.data.galleries;
                        this.link = response.data.link;
                        this.logo = response.data.logo;
                    }).catch(error => {
                    this.loading=false;
                    this.error=error.response.data
                    console.log(error.response.data)
                });
            },
            logout(event) {
                this.loading = true;
                var form = $(event.target)
                if (true) {
                    axios({
                        url: $(form).attr('action'),
                        method: 'post'
                    })
                        .then(response => {
                            this.loading = false;
                            console.log(response)
                            //location.reload();
                            this.$router.push({ path: "/" });
                            ////this.$router.go('/')
                            //window.location.href = location.host
                        }).catch(error => {
                            console.log(error)
                        //console.log(error.response.data)
                    });
                }
            }
        },
    }
</script>

<style scoped>
    @media (max-width: 576px)

    {
        .w-alert { width: 85% !important }
    }

    @media (min-width: 577px)

    {
        .w-alert { width: 25% !important }
    }

</style>