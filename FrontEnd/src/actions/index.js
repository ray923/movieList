import axios from 'axios';
const BaseUrl = 'https://movie-functions.azurewebsites.net';
const CancelToken = axios.CancelToken;
let cache = {};
// 请求拦截器中用于判断数据是否存在以及过期 未过期直接返回
axios.interceptors.request.use((config) => {
  // 如果需要缓存--考虑到并不是所有接口都需要缓存的情况
  if (config.cache && !config.refresh) {
    let source = CancelToken.source();
    config.cancelToken = source.token;
    // 去缓存池获取缓存数据
    let data = cache[config.url];
    // 获取当前时间戳
    let current_time = getCurrentTime();
    // 判断缓存池中是否存在已有数据 存在的话 再判断是否过期
    // 未过期 source.cancel会取消当前的请求 并将内容返回到拦截器的err中
    if (data && current_time - data.expire < config.expire_time) {
      source.cancel(data);
    }
  }
  return config;
});
// 响应拦截器中用于缓存数据 如果数据没有缓存的话
axios.interceptors.response.use(
  (response) => {
    // 只缓存get请求
    if (response.config.method === 'get' && response.config.cache) {
      // 缓存数据 并将当前时间存入 方便之后判断是否过期
      let data = {
        expire: getCurrentTime(),
        data: response
      };
      cache[`${response.config.url}`] = data;
    }
    return response;
  },
  (error) => {
    // 请求拦截器中的source.cancel会将内容发送到error中
    // 通过axios.isCancel(error)来判断是否返回有数据 有的话直接返回给用户
    if (axios.isCancel(error)) return Promise.resolve(error.message.data);
    // 如果没有的话 则是正常的接口错误 直接返回错误信息给用户
    return Promise.reject(error);
  }
);
// 获取当前时间
function getCurrentTime() {
  return new Date().getTime();
}

export function getAllMovies(page) {
  return axios
    .get(`${BaseUrl}/api/allmovies?page=${page}`, {
      cache: true,
      refresh: false,
      expire_time: 3600000
    })
    .then(res => { return res.data })
    .catch(err => { return err.message.data });
}

export function getMovieDetail(id) {
  return axios
    .get(`${BaseUrl}/api/movie?id=${id}`, {
      cache: true,
      refresh: false,
      expire_time: 3600000
    })
    .then(res => { return res.data })
    .catch(err => { return err.message.data });
}

export function getMovieDownload(id) {
  return axios
    .get(`${BaseUrl}/api/download?id=${id}`, {
      cache: true,
      refresh: false,
      expire_time: 3600000
    })
    .then(res => { return res.data })
    .catch(err => { return err.message.data });
}

export function getCategory(id, page) {
  return axios
    .get(`${BaseUrl}/api/category?id=${id}&page=${page}`, {
      cache: true,
      refresh: false,
      expire_time: 3600000
    })
    .then(res => { return res.data })
    .catch(err => { return err.message.data });
}

export function search(keyword, page) {
  return axios
    .get(`${BaseUrl}/api/search?keyword=${keyword}&page=${page}`, {
      cache: true,
      refresh: false,
      expire_time: 3600000
    })
    .then(res => { return res.data })
    .catch(err => { return err.message.data });
}

