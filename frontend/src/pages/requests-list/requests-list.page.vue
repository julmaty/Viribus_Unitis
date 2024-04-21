<script lang="ts">

import {computed, defineComponent} from "vue";

import { useMainDataStore } from '@/store/mainData.ts';

import { tagsListName } from "@/store/mainData.js";

export default defineComponent({

  setup(){

    const mainDataStore=useMainDataStore();

    const requestsList=computed(()=>mainDataStore.requestsList);

    return {

      mainDataStore,

      requestsList,

    }

  },


  data(){
    return{

      tagsList:tagsListName,

    }
  },

  computed:{

    anotherRequestsList(){

       return this.requestsList.sort(function(el1, el2){

        return (el1.time - el2.time)*(-1);

      });

    }

  },

  methods:{

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

  <div class="request-list">

    <router-link class="requestItem" :to="`request/${request.id}`" v-for="request in anotherRequestsList">

      <div class="name">{{ request.name }}</div>

      <div class="time">Размещено {{ requestDate(request.time) }}</div>

      <div class="tags-list">

        <div class="tag" v-for="tagId in request.tags">{{ searchTagNameById(tagId) }}</div>

      </div>

    </router-link>

  </div>


</template>

<style scoped lang="scss">

.request-list{

  overflow:auto;

  padding-bottom:30px;

  .requestItem{

    widht:100%;
    height:150px;
    cursor:pointer;
    display:block;
    margin-bottom:20px;

    border-color:black;
    border-style:solid;
    border-radius:8px;
    padding-left:15px;
    padding-right:10px;
    padding-top:15px;
    background-color:#EDEDED;

    .name{

      font-size:20px;
      font-weight:bold;
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

        margin-left:5px;
        margin-right:5px;
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

}

</style>