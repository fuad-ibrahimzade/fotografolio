<template>
    <div>
        <link href="/common-css/venobox.css" rel="stylesheet">

        <link href="/02_Portfolio/css/styles.css" rel="stylesheet">

        <link href="/02_Portfolio/css/responsive.css" rel="stylesheet">

        <link rel="stylesheet" href="/css/cocoen.min.css">

        <section class="main-section">
            <!--p-grid-isotope-->
            <div class="card-columns" style="column-count: 5">
                <!--p-item grid-sizer-->
                <div class=" card" v-for="photo in photos">
                    <!--venobox-->
                    <a class=" "  :href="photo.name">
                        <img class="card-img-top img-fluid" :src="photo.name" alt=""/>
                    </a>
                </div>
            </div><!-- p-grid -->
        </section><!-- main-section -->

        <script type="application/javascript">
            document.querySelectorAll('.cocoen').forEach(function(element){ new Cocoen(element); });
        </script>
    </div>
</template>

<script>
    // import HomeLayout from '@/components/home/HomeLayout.vue';

    export default {
        name: "Gallewry.vue",
        data() {
            return {
                loading: false,
                error: null,
                photos: null,
                oldRequestParam: null,
            };
        },
        mounted(){
            let script1 = document.createElement('script')
            script1.setAttribute('src', '/common-js/isotope.pkgd.min.js')
            document.getElementById('scripts').appendChild(script1)
            let script2 = document.createElement('script')
            script2.setAttribute('src', '/common-js/venobox.min.js')
            document.getElementById('scripts').appendChild(script2)
            let script3 = document.createElement('script')
            script3.setAttribute('src', '/js/cocoen.min.js')
            document.getElementById('scripts').appendChild(script3)


            if(!this.oldRequestParam || this.oldRequestParam!=this.$route.params.galleryname) {
                this.oldRequestParam=this.$route.params.galleryname
                this.startGettingData(this.$route.params.galleryname)
            }
        },
        methods:{
            setData(err, {data: photos}) {
                if (err) {
                    this.error = err.toString();
                } else {
                    this.photos = photos;
                }
            },
            startGettingData(galleryname){
                // this.loading=true;
                axios
                    .get('/api/gallery/'+galleryname)
                    .then(response => {
                        this.loading=false;
                        this.photos = response.data.photos;
                    }).catch(error => {
                    this.loading=false;
                    this.error=error.response.data
                    console.log(error.response.data)
                });
            },
        },
        watch: {
            '$route.params.galleryname'(newGalleryname, oldGalleryname) {
                if(false) {
                    var element=document.getElementById('scripts').getElementsByTagName('script')[0]
                    while(element){
                        // element.parentNode.removeChild(element);
                        element.remove()
                        element=document.getElementById('scripts').getElementsByTagName('script')[0]
                    }
                    // (elem=document.getElementById("testScriptName")).parentNode.removeChild(elem)

                    let script1 = document.createElement('script')
                    script1.setAttribute('src', '/common-js/isotope.pkgd.min.js')
                    document.getElementById('scripts').appendChild(script1)
                    let script2 = document.createElement('script')
                    script2.setAttribute('src', '/common-js/venobox.min.js')
                    document.getElementById('scripts').appendChild(script2)
                    let script3 = document.createElement('script')
                    script3.setAttribute('src', '/js/cocoen.min.js')
                    document.getElementById('scripts').appendChild(script3)
                }
                // location.reload()
                // this.$root.reload()
                this.startGettingData(newGalleryname)
            }
        }
    }
</script>

<style scoped>
    @media(max-width: 767px) {
        ::-webkit-scrollbar:vertical {
            width: 20px;
        }
    }

    @media(min-width: 768px) {
        ::-webkit-scrollbar:vertical {
            width: 15px;
        }
    }

    ::-webkit-scrollbar {
        -webkit-appearance: none;
    }

    ::-webkit-scrollbar:horizontal {
        height: 12px;
    }
    ::-webkit-scrollbar-thumb {
        background-color: rgba(0, 0, 0, .5);
        border-radius: 10px;
        border: 2px solid #ffffff;
    }
    ::-webkit-scrollbar-track {
        border-radius: 10px;
        background-color: #ffffff;
    }
</style>