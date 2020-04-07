<template>
    <div>
        <link href="06_Regular_Page/css/styles.css" rel="stylesheet">
        <link href="06_Regular_Page/css/responsive.css" rel="stylesheet">
        <section class="main-section" v-if="about">

            <div class="left-area">
                <h3 class="margin-tb-10">{{ about.title }}</h3>

                <p class="margin-b-10">{{ about.desc }}</p>

            </div><!-- left-area -->

            <div class="right-area">

                <img :src=" about.photo " alt="">

            </div><!-- right-area -->

        </section><!-- main-section -->
    </div>
</template>

<script>
    import axios from 'axios';

    export default {
        name: "About.vue",
        data() {
            return {
                loading: false,
                error: null,
                about: null,
            };
        },
        mounted(){
            this.startGettingData()
        },
        methods:{
            setData(err, {data: about}) {
                if (err) {
                    this.error = err.toString();
                } else {
                    this.about = about;
                }
            },
            startGettingData(){
                // this.loading=true;
                axios
                    .get('/api/about')
                    .then(response => {
                        this.loading=false;
                        this.about = response.data.about;
                    }).catch(error => {
                        this.loading=false;
                        this.error=error.response.data
                        console.log(error.response.data)
                });
            },
        }
    }
</script>

<style scoped>

</style>