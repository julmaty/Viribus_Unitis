<script lang="ts">

import {computed, defineComponent} from "vue";

import respondModalWindow from'./respond-modal-window.component.vue'
import {useMainDataStore} from "@/store/mainData";
import { tagsListName } from "@/store/mainData.js";
import axios from "axios";



export default defineComponent({


  setup(){

    const mainDataStore=useMainDataStore();

    const requestsList=computed(()=>mainDataStore.requestsList);


    return{

      mainDataStore,

      requestsList,

    }

  },


  created(){

    if(this.$route.name=='myRequest'){

      this.mainDataStore.setNameOfPage('Мой запрос №'+this.$route.params.id);

    }else{

      this.mainDataStore.setNameOfPage('Запрос №'+this.$route.params.id);

    }

    let requestId=this.$route.params.id;

    this.request={...this.requestsList.find(el=>el.id==requestId)};

  },



  data(){

    return{

      request:null,

      tagsList:tagsListName,

      respondModalWindowIsShow:false,

    }

  },

  methods:{

    searchTagNameById(id:number){

      return this.tagsList.find(tag => tag.id == id).name;

    },


    sendedRespond(){

      this.respondModalWindowIsShow=false;

    },

    closeModalWindow() {

      this.respondModalWindowIsShow=false;


    },

    openModalWindow(){

      this.respondModalWindowIsShow=true;

    },

    requestDate(time:Date){

      let day=time.getDate();//26
      let month=time.getMonth();//9
      let year=time.getFullYear();//2023
      let hours=time.getHours();//26
      let minutes=time.getMinutes();//9

      if(hours<10){

        hours=`0${hours}`;

      }

      if(minutes<10){

        minutes=`0${minutes}`;

      }


      if(day==new Date().getDate() && month==new Date().getMonth() && year==new Date().getFullYear()){

        return `cегодня в ${hours}:${minutes}`;

      }else{

        if(day<10){

          day=`0${day}`;

        }

        if(month<10){

          month=`0${month+1}`;

        }else{

          month=`${month+1}`;

        }

        return `${day}.${month}.${year} ${hours}:${minutes}`;

      }

    },

    async deleteRequest(){

      await axios.delete(`https://viribusunitis.ru/api/requests/${this.request.id}`);

      this.mainDataStore.requestsList=this.mainDataStore.requestsList.filter(el=>el.id!=this.request.id);

      this.$router.push({replace:true, path:'/myRequestsResponses'});

    },

  },

  computed:{

    backPageURL(){

      if(this.$route.name=='myRequest'){

        return '/myRequestsResponses';

      }else {

        return '/';

      }

    },

  },


  components:{

    respondModalWindow,

  },


});

</script>

<template>

  <div class="request">

    <router-link class="backButton" :to="backPageURL"><img src="@/assets/leftBottom.svg"></router-link>

    <div class="userFullName" v-if="$route.name!='myRequest'">Заказчик: {{ request.userName }}</div>

    <div class="name">Название запроса: {{ request.name }}</div>

    <div class="time">Размещено {{ requestDate(request.time) }}</div>

    <div class="tags-list">

      <div class="tag" v-for="tagId in request.tags">{{ searchTagNameById(tagId) }}</div>

    </div>

    <div class="comment">Описание запроса: {{request.comment}}</div>

    <div

        class="respondButton"

        v-if="!respondModalWindowIsShow && $route.name!='myRequest' && this.request.userId!=mainDataStore.userId"

        v-on:click="openModalWindow"

    >Откликнуться</div>

    <div

        class="respondButton"

        v-if="!respondModalWindowIsShow && $route.name=='myRequest' && this.request.userId==mainDataStore.userId"

        v-on:click="deleteRequest"

    >Удалить</div>


    <respondModalWindow

        v-if="respondModalWindowIsShow"

        v-on:sendedRespond="sendedRespond"

        v-on:closeModalWindow="closeModalWindow"

        :requestId="request.id"

    />

  </div>


</template>

<style scoped>


.request{

  .backButton{

    font-size:20px;
    cursor:pointer;
    display:block;

  }

  .userFullName{

    font-size:20px;
    margin-top:20px;
    margin-bottom:20px;

  }

  .name{

    font-size:20px;
    margin-top:20px;
    margin-bottom:20px;

  }

  .time{

    font-size:12px;
    color:gray;
    margin-top:10px;

  }


  .comment{

    font-size:18px;

  }

  .tags-list{

    display:flex;
    margin-top:20px;
    margin-bottom:20px;
    flex-wrap: wrap;

    .tag{

      margin-left:10px;
      margin-right:10px;
      color:#3643B3;
      border-width: 1px;
      border-color:#3643B3;
      border-style:solid;
      border-radius:10px;
      padding:5px;
      font-size:18px;
      margin-bottom:10px;

    }

  }

  .respondButton{


    margin-left:auto;
    margin-right:auto;
    margin-top:30px;
    widht:130px;
    background-color: red;
    font-size:20px;
    border-radius:20px;
    height:50px;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    margin-bottom:60px;

    color:white;

    background-color:#3643B3;

  }

}


</style>