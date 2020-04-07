<template>
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-md-8">
                <div class="card">
                    <div class="card-header">Login</div>

                    <div class="card-body">
                        <form method="POST" action="/login" @submit.stop.prevent="checkForm($event)" novalidate="true">
                            <input type="hidden" name="__RequestVerificationToken" :value="this.$root.csrf_token">

                            <p v-if="errors.length">
                                <b>Please correct the following error(s):</b>
                                <ul>
                                    <li v-for="error in errors">{{ error }}</li>
                                </ul>
                            </p>

                            <div class="form-group row">
                                <label for="email" class="col-sm-4 col-form-label text-md-right">E-Mail Address</label>

                                <div class="col-md-6">
                                    <input id="email" type="email" :class="'form-control'+errors.email ? ' is-invalid' : '' " name="email"  v-model="email" required autofocus>

                                    <!--@if ($errors->has('email'))-->
                                    <!--<span class="invalid-feedback" role="alert">-->
                                        <!--<strong>$errors->first('email')</strong>-->
                                    <!--</span>-->
                                    <!--@endif-->
                                </div>
                            </div>

                            <div class="form-group row">
                                <label for="password" class="col-md-4 col-form-label text-md-right">Password</label>

                                <div class="col-md-6">
                                    <input id="password" type="password" :class="'form-control'+errors.password ? ' is-invalid' : '' " name="password" required>

                                    <!--@if ($errors->has('password'))-->
                                    <!--<span class="invalid-feedback" role="alert">-->
                                        <!--<strong>$errors->first('password')</strong>-->
                                    <!--</span>-->
                                    <!--@endif-->
                                </div>
                            </div>

                            <div class="form-group row">
                                <div class="col-md-6 offset-md-4">
                                    <div class="form-check">
                                        <input class="form-check-input" type="checkbox" name="remember" id="remember" :checked="remember?true:false">

                                        <label class="form-check-label" for="remember">
                                            Remember Me
                                        </label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group row mb-0">
                                <div class="col-md-8 offset-md-4">
                                    <button type="submit" class="btn btn-primary">
                                        Login
                                    </button>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from "axios"
    //axios.defaults.withCredentials = true;

    export default {
        name: "Login.vue",
        data() {
            return {
                loading: false,
                error: null,
                errors:[],
                email: null,
                remember: false,
                galleries: null,
            };
        },
        created(){
            var isLoggedIn = $("meta[name=login-status]").attr('content');
            if(isLoggedIn){
                this.$router.push({path:'/dashboard'})
            }
        },
        methods:{
            checkForm: function (e) {
                this.errors = [];

                if (!this.email) {
                    this.errors.push('Email required.');
                } else if (!this.validEmail(this.email)) {
                    this.errors.push('Valid email required.');
                }
                this.$parent.loading = true;
                var form=$(event.target)
                if (!this.errors.length) {
                    axios({
                        url: $(form).attr('action'),
                        method: 'post',
                        data:new FormData(event.target)
                    })
                        .then(response => {
                            this.$parent.loading = false;
                            console.log(response)
                            //this.$router.push({ path: '/dashboard' })
                            //this.$router.push('dashboard') 
                            location.reload();
                        }).catch(error => {
                        this.$parent.loading=false;
                        this.error=error.response.data
                        console.log(error.response.data)
                    });
                    //return true;
                }

                //e.preventDefault();
            },
            validEmail: function (email) {
                var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                return re.test(email);
            }
        }
    }
</script>

<style scoped>

</style>