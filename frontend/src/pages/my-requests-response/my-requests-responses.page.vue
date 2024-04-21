<script lang="ts">

import {computed, defineComponent} from "vue";
import {useMainDataStore} from "@/store/mainData";

import { tagsListName } from "@/store/mainData.js";


export default defineComponent({


  setup(){

    const mainDataStore=useMainDataStore();

    const requestsList=computed(()=>mainDataStore.requestsList);

    const myResponsesList=computed(()=>mainDataStore.myResponsesList);


    const userId=computed(()=>mainDataStore.userId);

    return {

      myResponsesList,

      mainDataStore,

      requestsList,

      userId,

    }

  },


  data(){
    return{

      tagsList:tagsListName,

      responsesList:[{


      }],

      requestsListShow:true,

    }
  },

  computed:{

    myRequestsList(){

      return this.requestsList.filter((el)=>el.userId==this.userId).sort(function(el1, el2){

        return (el1.time - el2.time)*(-1);

      });

    },

  },


  methods:{

    setRequestsListStatus(newStatus:boolean){

      this.requestsListShow=newStatus;

    },

    searchTagNameById(id:number){

      return this.tagsList.find(tag => tag.id == id).name;

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

  },


});

</script>

<template>

  <div class="container">

    <router-link class="backButton" to="/"><img src="@/assets/leftBottom.svg"></router-link>

    <div class="choose-block">

        <div :class="{active:requestsListShow==true}" v-on:click="setRequestsListStatus(true)">Мои запросы</div>

        <div :class="{active:requestsListShow==false}" v-on:click="setRequestsListStatus(false)">Мои отклики</div>

      </div>


    <div class="list-container">

      <div class="request-list" v-if="requestsListShow==true">

        <router-link class="requestItem" :to="`/myRequestsResponses/request/${request.id}`" v-for="request in myRequestsList">

          <div class="name">{{ request.name }}</div>

          <div class="time">Размещено {{ requestDate(request.time) }}</div>

          <div class="tags-list">

            <div class="tag" v-for="tagId in request.tags">{{ searchTagNameById(tagId) }}</div>

          </div>

        </router-link>

      </div>

      <div class="response-list" v-if="requestsListShow==false">

        <router-link class="requestItem" :to="`/myRequestsResponses/response/${response.id}`" v-for="response in myResponsesList">

          <div class="name">{{ response.request.name }}</div>

          <div class="time">Размещено {{ requestDate(response.request.time) }}</div>

          <div class="tags-list">

            <div class="tag" v-for="tagId in response.request.tags">{{ searchTagNameById(tagId) }}</div>

          </div>

        </router-link>

      </div>

    </div>


  </div>

</template>

<style scoped>

.backButton{

  font-size:20px;
  cursor:pointer;
  display:block;

}

.choose-block{
  display:flex;
  width:80%;
  height:60px;
  align-items:center;
  margin-left:auto;
  justify-content:space-evenly;
  margin-right:auto;

  div{

    font-size:20px;
    cursor:pointer;

  }


  .active{

    color:#3643B3;

  }

}


.list-container{

  height:calc(100% - 80px);
  width:100%;
  overflow:auto;

  .request-list,.response-list{

    width:100%;
    height:100%;

  }

}

.requestItem{

  widht:100%;
  height:150px;
  cursor:pointer;
  display:block;
  margin-bottom:20px;

  border-color:black;
  border-style:solid;
  border-radius:10px;
  padding-left:10px;
  padding-right:10px;
  padding-top:10px;
  background-color:#EDEDED;

  .name{

    font-size:20px;
  }

  .time{

    font-size:12px;
    color:gray;
    margin-top:10px;

  }

  .tags-list{

    display:flex;
  //background-color: yellow;
    margin-top:10px;
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
      font-size:16px;
      margin-bottom:10px;

    }

  }

}


</style>