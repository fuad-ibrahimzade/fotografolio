<template>
  <div>
    <div class="container">
      <div class="row justify-content-center">
        <div class="col-md-8">
          <div class="card">
            <div class="card-header">Link Section</div>

            <div class="card-body">
              <div class="alert alert-success" role="alert" v-if="status">
                {{ status }}
              </div>

              <form method="post" action="/link/update" @submit.prevent.stop="updateLinkMethod($event)">
                <!--<input type="hidden" name="_token" :value="csrf_token">-->
                <div class="form-row mb-2">
                  <div class="col-12 mb-2">
                    <label>Facebook:</label>
                    <input type="text" name="facebook" :value="link[0].name" maxlength="100" class="form-control" required>
                  </div>
                  <div class="col-12 mb-2">
                    <label>Instagram:</label>
                    <input type="text" name="instagram" :value="link[1].name" maxlength="100" class="form-control" required>
                  </div>
                  <div class="col-12 mb-2">
                    <label>Twitter:</label>
                    <input type="text" name="twitter" :value="link[2].name" maxlength="100" class="form-control" required>
                  </div>
                  <div class="col-12 mb-2">
                    <label>Youtube:</label>
                    <input type="text" name="youtube" :value="link[3].name" maxlength="100" class="form-control" required>
                  </div>
                  <div class="col-12 mb-2">
                    <button type="submit" class="form-control btn btn-primary">Confirm</button>
                  </div>
                </div>
              </form>


            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
    export default {
        name: "Link",
        data() {
            return {
                csrf_token:this.$root.csrf_token,
                api_token:this.$root.api_token,
                loading: false,
                error: null,
                status:null,
                link:null,
            };
        },
        mounted(){
            this.startGettingData()
        },
        methods:{
            updateLinkMethod(event){
                this.$parent.loading=true;
                var form=$(event.target)
                axios({
                    url: '/api' + $(form).attr('action'),
                    method: 'post',
                    headers: {
                        'X-CSRF-TOKEN':this.csrf_token,
                        'X-Requested-With':'XMLHttpRequest',
                        'Authorization':'Bearer ' + this.api_token,
                    },
                    data:new FormData(event.target)
                })
                    .then(response => {
                        this.$parent.loading=false;
                        if(response.data.hasOwnProperty('redirectTo')){
                            this.redirectTo=response.data.redirectTo
                            this.$router.push({path:this.redirectTo});
                        }
                        if(response.data.hasOwnProperty('fileError')){
                            this.fileError=response.data.fileError
                        }
                        this.startGettingData()
                    }).catch(error => {
                    this.$parent.loading=false;
                    this.error=error.response.data
                    console.log(error.response.data)
                });
            },
            startGettingData(){
                this.loading=true;
                axios
                    .get('/api/link',{
                        headers: {
                            'X-CSRF-TOKEN': this.csrf_token,
                            'X-Requested-With': 'XMLHttpRequest',
                            'Authorization': 'Bearer ' + this.api_token,
                        }
                    })
                    .then(response => {
                        this.loading=false;
                        this.link = response.data.link
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