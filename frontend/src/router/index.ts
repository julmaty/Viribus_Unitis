import { createRouter, createWebHistory } from 'vue-router'

import requestsList from '@/pages/requests-list/requests-list.page.vue';

import request from '@/pages/request/request.page.vue';
import response from '@/pages/response/response.page.vue';

import createRequest from '@/pages/create-request/create-request.page.vue';
import myRequestsResponses from '@/pages/my-requests-response/my-requests-responses.page.vue';
import { useMainDataStore } from '@/store/mainData.ts'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),

  routes: [

    {

      path: '/request/:id',
      name: 'request',
      component: request,
      meta:{nameOfPage:'Запрос'},

    },

    {

      path: '/requestsList',
      name: 'requestsList',
      component: requestsList,
      meta:{nameOfPage:'Список запросов'},

    },

    {

      path: '/',

      //При попытке перейти на стартовую страницу приложения
      //Переходим на соответствующую начальную страницу

        redirect:(to)=>{

          return {path:'/requestsList'}

        },

    },

    {
      path: '/createRequest',
      name: 'createRequest',
      component: createRequest,
      meta:{nameOfPage:'Создать запрос'},

    },

    {

      path: '/myRequestsResponses',
      name: ' myRequestsResponses',
      component: myRequestsResponses,
      meta:{nameOfPage:'Мои ответы и запросы'},

    },

    {

      path: '/myRequestsResponses/request/:id',
      name: 'myRequest',
      component: request,
      meta:{nameOfPage:'Мой запрос'},

    },

    {

      path: '/myRequestsResponses/response/:id',
      name: 'myResponse',
      component: response,
      meta:{nameOfPage:'Мой отклик'},

    },

  ]
});

//Регистрируем глобальный защитник путей
router.beforeEach((to, from)=>{

  //Если пользователь вводит несуществующий путь, то происходит редирект на главную
  //страницу, а с нее идет редирект куда нужно
  if(to.matched.length === 0){

    return{path:'/', replace:true};

  }

  let mainDataStore=useMainDataStore();


  //Получаем информацию о необходимом статусе авторизации
  //для доступа к данной странице
  let pageName=to.meta.pageName;

  mainDataStore.setNameOfPage(to.meta.nameOfPage);

});


export default router
