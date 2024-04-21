<script lang="ts">

import {computed, defineComponent} from "vue";

import { useMainDataStore } from '@/store/mainData.ts';
import axios from "axios";

export default defineComponent({

  setup(){

    const mainDataStore=useMainDataStore();

    const nameOfPage=computed(()=>mainDataStore.nameOfPage);

    const token=computed(()=>mainDataStore.token);

    const userId=computed(()=>mainDataStore.userId);

    const errorText=computed(()=>mainDataStore.errorText);



        return{

          userId,

      mainDataStore,

      nameOfPage,

      errorText,

          token,

    }

  },

  created(){

    const urlParams = new URLSearchParams(window.location.search);

    let token = urlParams.get('token');

    //Токен Юли
    //token='zoYAlMokyGjIwVbCtfRJwABRmJmqHARIxybgZuDwXgDDTRxguOUnVjFMhkgtwxky';


    //Токен Дани
    token='VkUDToYDlVTPlppmtYlSoQnRdxfakfwJbmsdcfwBzGlwrVjWNYfcHeVBSnHLQfYY';

    this.mainDataStore.setToken(token);

    //Получаем основные данные для работы приложения
    this.getMainData();

  },

  methods:{

    async getMainData(){

      //Получаем по токену id пользователя
     let response= await axios.get('https://api.test.profcomff.com/auth/me?info=groups',
          {headers:{ 'Authorization': this.token,}
          });

        const userId=response.data.id;

        this.mainDataStore.email=response.data.email;

        this.mainDataStore.setUserId(userId);



      //Получаем список запросов
      response= await axios.get('https://viribusunitis.ru/api/requests');

      console.log(response.data);

      let requestsList=response.data.map((el:any)=>{

        let tags=el.tags.map((tag:any)=>{

          return tag.id;
        });

        return{

          id:el.id,
          name:el.name,
          userId:el.userId,
          comment:el.description.text,
          time:new Date(el.createdDate),
          tags:tags,

         };

      });

      this.mainDataStore.setRequestsList(requestsList);

      //Получаем список откликов
      response= await axios.get('https://viribusunitis.ru/api/responses');

      console.log(response.data);

      let myResponsesList=response.data.filter((el:any)=>el.userId==this.userId);

      myResponsesList=myResponsesList.map((el:any)=>{

        let time=new Date(el.request.createdDate);

        let tags=el.request.tags.map((tag:any)=>{

          return tag.id;

        });

        return{

          id:el.id,
          phone:el.phone,
          comment:el.comment,
          requestId:el.requestId,
          request:{...el.request, time, tags},
        };

      });

      this.mainDataStore.setMyResponsesList(myResponsesList);

      this.mainDataStore.dataHasBeenUploaded=true;

    },

  }




});

</script>

<template>

  <div class="pageName">

    {{ mainDataStore.nameOfPage }}

    <router-link

        to="/createRequest"

        class="add-request-button"

         v-if="nameOfPage=='Список запросов'">

          +

    </router-link>


    <router-link

        to="/myRequestsResponses"

        class="my-response-request-button"

        v-if="nameOfPage=='Список запросов'">

        <img src='@/assets/photo_5271804075459074748_x.jpg'>

    </router-link>

  </div>

  <router-view class="page" v-if="mainDataStore.dataHasBeenUploaded==true"/>

  <div class="error" v-if="errorText.length>0">{{ errorText }}</div>

</template>

<style scoped lang="scss">


.pageName{

  width:100%;
  height:50px;
  font-size:20px;
  display:flex;
  justify-content: center;
  align-items:center;
  padding-top:10px;


  .add-request-button{

    position:absolute;
    right:30px;
    //top:10px;
    font-size:30px;
    cursor:pointer;

  }

  .my-response-request-button{

    position:absolute;
    right:60px;
    top:15px;
    //top:10px;
    font-size:30px;
    cursor:pointer;

    img{

      height:25px;
      width:25px;

    }

  }

}

.page{

  padding-top:10px;
  height:calc(100% - 20px);
  widht:100%;
  padding-left:15px;
  padding-right:15px;
  overflow:auto;

}

.error{

  position:absolute;
  margin-left:auto;
  margin-right:auto;
  right:0px;
  left:0px;
  bottom:20px;
  display:flex;
  align-items:center;
  justify-content:center;
  font-size:20px;
  text-align:center;


  width:80%;
  height:50px;
  border-color:black;
  border-width:1px;
  background:black;
  color:white;
  border-radius:20px;



}


</style>
