<template>
    <div>
        <!--<link href="/common-css/bootstrap.css" rel="stylesheet">-->
        <!--<link rel="stylesheet" type="text/css" href="/revolution/css/settings.css">-->
        <!--<link href="/01_Photography/css/styles.css" rel="stylesheet">-->
        <!--<link href="/01_Photography/css/responsive.css" rel="stylesheet">-->
        <section class="slider-section">

            <div id="rev_slider_28_1_wrapper" class="rev_slider_wrapper fullscreen-container" data-alias="parallax-zoom-slices" data-source="gallery" style="background:#000000;padding:0px;">
                <div id="rev_slider_28_1" class="rev_slider fullscreenbanner" style="display:block;" data-version="5.4.1">
                    <slider animation="fade" width='auto' height='660px' duration="4000" indicators="false">
                            <slider-item v-for="photo in photos" >
                                <img :src="photo.name"  alt="" class="center-cropped-img">
                            </slider-item>
                    </slider>
                    <div class="tp-bannertimer" style="height: 10px; background: rgba(255,255,255,0.25);"></div>    </div>
            </div><!-- END REVOLUTION SLIDER -->

        </section><!-- slider -->
        <!--<parent>-->
            <!--<script type="application/javascript" src="/common-js/tether.min.js"></script>-->

            <!--<script type="application/javascript" src="/common-js/bootstrap.js"></script>-->

            <!--&lt;!&ndash; REVOLUTION JS FILES &ndash;&gt;-->
            <!--<script type="application/javascript" src="/revolution/js/jquery.themepunch.tools.min.js"></script>-->
            <!--<script type="application/javascript" src="/revolution/js/jquery.themepunch.revolution.min.js"></script>-->
        <!--</parent>-->
    </div>
</template>

<script>
    // import HomeLayout from '@/components/home/HomeLayout.vue';
    import axios from 'axios';
    const getPhotoData = (page, callback) => {
        const params = { page };

        axios
            .get('/api/photos')
            .then(response => {
                callback(null, response.data);
            }).catch(error => {
            callback(error, error.response.data);
        });
    };

    export default {
        name: "HomeIndex.vue",
        data() {
            return {
                loading: false,
                error: null,
                photos: null,
            };
        },
        mounted(){
            this.startGettingData()
            let script10 = document.createElement('script')
            script10.setAttribute('src', '/common-js/jquery-3.2.1.min.js')
            // script10.type='text/javascript'
            document.head.appendChild(script10)
            let script1 = document.createElement('script')
            script1.setAttribute('src', '/common-js/tether.min.js')
            document.getElementById('scripts').appendChild(script1)
            let script2 = document.createElement('script')
            script2.setAttribute('src', '/common-js/bootstrap.js')
            document.getElementById('scripts').appendChild(script2)
            // let script3 = document.createElement('script')
            // script3.type='text/javascript'
            // script3.setAttribute('src', '/revolution/js/jquery.themepunch.tools.min.js')
            // document.getElementById('scripts').appendChild(script3)
            // let script4 = document.createElement('script')
            // script4.type='text/javascript'
            // script4.setAttribute('src', '/revolution/js/jquery.themepunch.revolution.min.js')
            // document.getElementById('scripts').appendChild(script4)
            //
            //
            // script3 = document.createElement('script')
            // script3.type='text/javascript'
            // script3.setAttribute('src', '/revolution/js/jquery.themepunch.tools.min.js')
            // document.head.appendChild(script3)
            // script4 = document.createElement('script')
            // script4.type='text/javascript'
            // script4.setAttribute('src', '/revolution/js/jquery.themepunch.revolution.min.js')
            // document.head.appendChild(script4)
        },
        methods:{
            setData(err, {data: photos}) {
                if (err) {
                    this.error = err.toString();
                } else {
                    this.photos = photos;
                }
            },
            startGettingData(){
                // this.loading=true;
                axios
                    .get('/api/photos')
                    .then(response => {
                        this.loading=false;
                        this.photos = response.data.photos;
                    }).catch(error => {
                        this.loading=false;
                        this.error=error.response.data
                        console.log(error.response.data)
                });
            }
            ,
        }
    }
    // extends: parent,
    //     components: {
    //     parent: HomeLayout
    // },
</script>

<style scoped>
    .center-cropped {
        background-position: center center;
        background-repeat: no-repeat;
    }
    .center-cropped img {
        min-height: 100%;
        min-width: 100%;
    }
    .center-cropped-img {
        object-fit: cover; /* Do not scale the image */
        object-position: center; /* Center the image within the element */
        height: inherit;
        width: inherit;
        animation: imganim 14s infinite linear both;
    }
    @keyframes imganim {
        from {transform: scale(1.0);}
        to {transform: scale(1.5);}
    }
    /*.center-cropped-img:hover {*/
        /*transform: scale(1.5); !* (150% zoom - Note: if the zoom is too large, it will go outside of the viewport) *!*/
    /*}*/
    /*@import "#/common-css/bootstrap.css";*/

    /*!*<!-- REVOLUTION STYLE SHEETS -->*!*/
    /*@import "#/revolution/css/settings.css";*/

    /*@import  "#/01_Photography/css/styles.css";*/
    /*@import "#/01_Photography/css/responsive.css";*/
</style>