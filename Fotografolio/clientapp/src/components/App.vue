<template>
    <div id="app">
        <!--<nav class="navbar navbar-expand-md navbar-light navbar-laravel">-->
            <!--<div class="container">-->
                <!--<ul class="navbar-nav">-->
                    <!--<router-link :to="{ name: 'home.layout' }" class="nav-link">Home</router-link>-->
                    <!--<router-link :to="{ name: 'page' }" class="nav-link" >Spa-Page</router-link>-->
                    <!--<router-link :to="{ name: 'users.index' }" class="nav-link">Users</router-link>-->
                <!--</ul>-->
            <!--</div>-->
        <!--</nav>-->
        <main>
            <router-view></router-view>
        </main>
    </div>
</template>
<script>
    export default {
        name: 'App.vue'
    }
</script>