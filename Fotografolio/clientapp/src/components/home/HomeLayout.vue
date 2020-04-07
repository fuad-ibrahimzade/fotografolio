<template>
    <div>
        <link href="https://fonts.googleapis.com/css?family=Poppins:400,500,600,700%7CAllura" rel="stylesheet">
        <link href="/common-css/ionicons.css" rel="stylesheet">

        <link href="/common-css/bootstrap.css" rel="stylesheet">
        <link rel="stylesheet" type="text/css" href="/revolution/css/settings.css">
        <link href="/01_Photography/css/styles.css" rel="stylesheet">
        <link href="/01_Photography/css/responsive.css" rel="stylesheet">
        <header>
            <router-link class="logo little" to="/" v-if="logo">
                <img :src="logo.photo" alt="Logo">
            </router-link>
            <ul class="social-icons right-area" v-if="link">
                <!--@if(isset($link[0]))-->
                <li><a :href="'https://www.facebook.com/'+link[0].name" target="_blank"><i class="ion ion-social-facebook-outline"></i></a></li>
                <li><a :href="'https://www.instagram.com/'+link[1].name" target="_blank"><i class="ion ion-social-instagram-outline"></i></a></li>
                <li><a :href="'https://twitter.com/'+link[2].name" target="_blank"><i class="ion ion-social-twitter-outline"></i></a></li>
                <li><a :href="'https://www.youtube.com/channel/'+link[3].name" target="_blank"><i class="ion ion-social-youtube-outline"></i></a></li>
                <!--@endif-->
            </ul><!-- social-icons -->

            <a class="menu-nav-icon lit" data-menu="#main-menu"><i class="ion-navicon"></i></a>

            <ul class="main-menu" id="main-menu">
                <li class="drop-down">
                    <a href="" onclick="event.preventDefault();return false;" >Portfolio<i class="ion-arrow-down-b"></i></a>
                    <ul class="drop-down-menu drop-down-inner">
                        <!--@fgoreach($galleries as $gallery)-->
                        <li v-for="gallery in galleries">
                            <!--<a :href="'/gallery/'+gallery.name" @click="swapComponent(componentsArray[1])">{{ gallery.name }}</a>-->
                            <!--<router-link :to="'/gallery/'+gallery.name">{{ gallery.name }}</router-link>-->
                            <!--<router-link :to="{ name: 'gallery', params: { galleryname: gallery.name }}">{{gallery.name}}</router-link>-->
                            <router-link :to="`/gallery/${gallery.name}`" :key="$route.fullPath">{{ gallery.name }}</router-link>
                            <!--<router-link :to="`/gallery/${gallery.name}`" @click="showGallery(gallery.name)" :key="$route.fullPath">{{ gallery.name }}</router-link>-->

                        </li>
                        <!--@endforeach-->
                    </ul>
                </li>
                <li><router-link to="/about">About</router-link></li>
                <li><router-link to="/contact">Contact</router-link></li>
            </ul>
        </header>
        <div class="content " id="content">
            <keep-alive>
                <component v-bind:is="currentLinkComponent" ref="childComponent"></component>
            </keep-alive>
            <!--<div :is="currentLinkComponent" ref="childComponent"></div>-->
            <!--<homeindex></homeindex>-->
        </div>
        <footer style="position: relative; width:100%; clear: both;" class="">
            <p class="copyright">
                Copyright &copy; <script type="application/javascript">document.getElementsByClassName('copyright')[0].innerText=(new Date().getFullYear())+' All rights reserved';</script> All rights reserved
            </p>

        </footer>
        <div id="scripts"></div>
        <script type="application/javascript">
            !function(f,b,e,v,n,t,s)
            {if(f.fbq)return;n=f.fbq=function(){n.callMethod?
                n.callMethod.apply(n,arguments):n.queue.push(arguments)};
                if(!f._fbq)f._fbq=n;n.push=n;n.loaded=!0;n.version='2.0';
                n.queue=[];t=b.createElement(e);t.async=!0;
                t.src=v;s=b.getElementsByTagName(e)[0];
                s.parentNode.insertBefore(t,s)}(window,document,'script',
                'https://connect.facebook.net/en_US/fbevents.js');
            fbq('init', '410159343058376');
            fbq('track', 'PageView');
        </script>
        <!--<script type="application/javascript">-->
            <!--$(document).ready(function() {-->
                <!--// console.log('height: '+parseInt($('#content').css('height').replace('px','')));-->
                <!--if (parseInt($('#content').css('height').replace('px',''))<692) {-->
                    <!--$('footer').css('margin-top', 652-parseInt($('#content').css('height').replace('px',''))+'px');-->
                <!--}-->
                <!--else{-->
                    <!--$('footer').css('margin-top', '15px')-->
                <!--}-->
                <!--setInterval(function () {-->
                    <!--// alert("Hello");-->
                    <!--if (parseInt($('#content').css('height').replace('px',''))<692) {-->
                        <!--$('footer').css('margin-top', 652-parseInt($('#content').css('height').replace('px',''))+'px')-->
                    <!--}-->
                    <!--else {-->
                        <!--$('footer').css('margin-top', '15px')-->
                    <!--}-->
                <!--}, 100);-->
            <!--})-->
        <!--</script>-->
        <!--<script type="application/javascript" src="/common-js/jquery-3.2.1.min.js"></script>-->
        <!--<script type="application/javascript" src="/common-js/scripts.js"></script>-->
    </div>
</template>

<script>
    import HomeIndex from '@/components/home/HomeIndex.vue';
    import Gallery from '@/components/home/Gallery.vue';
    import About from '@/components/home/About.vue';
    import Contact from '@/components/home/Contact.vue';
    import axios from 'axios';
    const getHomeData = (page, callback) => {
        const params = { page };

        axios
            .get('/api/homedata', { params })
            .then(response => {
                callback(null, response.data);
            }).catch(error => {
            callback(error, error.response.data);
        });
    };
    export default {
        name: "HomeLayout.vue",
        data() {
            return {
                loading: false,
                error: null,
                link: this.$root.link,
                logo: this.$root.logo,
                galleries: this.$root.galleries,
                currentLinkComponent: HomeIndex,
                componentsArray: [HomeIndex,Gallery,About,Contact],
            };
        },
        components: { homeindex:HomeIndex},
        created() {
            document.title = 'Photographer -'
            // this.noscriptHtml = '<noscript> <img height="1" width="1" src="https://www.facebook.com/tr?id=410159343058376&ev=PageView&noscript=1"/>asdasdsad</noscript>';
        },
        mounted() {
            let script1 = document.createElement('script')
            script1.setAttribute('src', '/common-js/jquery-3.2.1.min.js')
            // script1.type='text/javascript'
            document.getElementById('scripts').parentNode.insertBefore(script1,document.getElementById('scripts'))
            let script2 = document.createElement('script')
            script2.setAttribute('src', '/common-js/scripts.js')
            document.body.appendChild(script2)


            // this.swapComponent(this.componentsArray[0])
            // this.$refs.childComponent.startGettingData()
            // if(!this.link || !this.logo || !this.galleries){
            //     this.startGettingData()
            // }
            // this.$router.push({ path: 'index' });

        },
        beforeRouteEnter (to, from, next) {
            if(!this.link || !this.logo || !this.galleries){
                getHomeData(to.query.page, (err, data) => {
                    next(vm => vm.setData(err, data));
                });
            }
        },
        beforeRouteUpdate (to, from, next) {
            // this.galleries = this.link = this.logo= null
            // getHomeData(to.query.page, (err, data) => {
            //     this.setData(err, data);
            //     next();
            // });
        },
        methods: {
            setData(err, {data: galleries, link, logo}) {
                if (err) {
                    this.error = err.toString();
                } else {
                    this.$root.galleries = galleries;
                    this.$root.link = link;
                    this.$root.logo = logo;
                    // console.log(this.galleries)
                }
            },
            swapComponent: function(component)
            {
                this.currentLinkComponent= component;
            },
            startGettingData(){
                // this.loading=true;
                axios
                    .get('/api/homedata')
                    .then(response => {
                        this.loading=false;
                        this.$root.galleries = response.data.galleries;
                        this.$root.link = response.data.link;
                        this.$root.logo = response.data.logo;

                        $(document).ready(function() {
                            // console.log('height: '+parseInt($('#content').css('height').replace('px','')));
                            if (parseInt($('#content').css('height').replace('px',''))<692) {
                                $('footer').css('margin-top', 652-parseInt($('#content').css('height').replace('px',''))+'px');
                            }
                            else{
                                $('footer').css('margin-top', '15px')
                            }
                            setInterval(function () {
                                // alert("Hello");
                                if (parseInt($('#content').css('height').replace('px',''))<692) {
                                    $('footer').css('margin-top', 652-parseInt($('#content').css('height').replace('px',''))+'px')
                                }
                                else {
                                    $('footer').css('margin-top', '15px')
                                }
                            }, 100);
                        })
                    }).catch(error => {
                        this.loading=false;
                        this.error=error.response.data
                        console.log(error.response.data)

                });
            },
            showGallery(galleryname){
                $router.push({ 'name': 'gallery', 'params': { 'galleryname': galleryname } })
                this.$refs.childComponent.startGettingData(galleryname)
            }
        }
    }
</script>

<style >
    /*@import "https://fonts.googleapis.com/css?family=Poppins:400,500,600,700%7CAllura";*/
    /*@import "#/common-css/ionicons.css";*/

    .ion { font-size: 25px }
    @media(min-width: 360px) and (max-width: 578px) {
        .little{ width: 25% }
    }
    @media(max-width: 359px) {
        .little{ width: 25%; position: relative; right: 25px; }
        .lit{ position: relative; left: 10px; }
    }
    .maincontent{
        min-height: 652px
    }
</style>