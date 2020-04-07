<template>
  <div>
    <div class="container">
      <div class="row justify-content-center">
        <div class="col-md-8">
          <div class="card">
            <div class="card-header">Logo Section</div>

            <div class="card-body">
              <div class="alert alert-success" role="alert" v-if="status">
                {{ status }}
              </div>

              <form method="post" action="/logo/update" enctype="multipart/form-data" @submit.prevent.stop="updateLogoMethod($event)">
                <!--<input type="hidden" name="_token" :value="csrf_token">-->
                <div class="form-row mb-2">
                  <div class="col-12 col-md-9 mb-2">
                    <input type="file" name="photo" accept="image/*" class="form-control" required>
                  </div>
                  <div class="col">
                    <input type="submit" value="Update" class="form-control btn btn-primary">
                  </div>
                </div>
              </form>


            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="modal fade" id="fileError" tabindex="-1" role="dialog" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Yükləməniz uğursuz oldu!</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            {{ fileError }}
          </div>
        </div>
      </div>
    </div>

    <script type="application/javascript" v-if="fileError">
        $(document).ready(function(){
            $('#fileError').modal('show');
        });
    </script>
  </div>
</template>

<script>
    export default {
        name: "Logo",
        data() {
            return {
                csrf_token:this.$root.csrf_token,
                api_token:this.$root.api_token,
                loading: false,
                error: null,
                fileError:null,
                status:null,
            };
        },
        mounted(){
            this.startGettingData()
        },
        methods:{
            updateLogoMethod(event){
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
                        // this.startGettingData()
                    }).catch(error => {
                    this.$parent.loading=false;
                    this.error=error.response.data
                    console.log(error.response.data)
                });
            },
        }
    }
</script>

<style scoped>

</style>