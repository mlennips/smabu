
import axios from 'axios';

const axiosConfig = axios.create({
    baseURL: 'http://localhost:5035/',
    maxBodyLength: Infinity,
});

axiosConfig.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('auth_idtoken')}`;

axiosConfig.interceptors.request.use(
    config => {
        const idToken = localStorage.getItem('auth_idtoken');
        config.headers.Authorization = `Bearer ${idToken}`;
        return config;
    },
    error => Promise.reject(error)
);

axiosConfig.interceptors.response.use(async (response) => {
    if (process.env.NODE_ENV === 'development') {
        await new Promise((resolve) => setTimeout(resolve, 25));
    }
    return response;
});

axiosConfig.interceptors.request.use(async (request) => {
    if (process.env.NODE_ENV === 'development') {
        await new Promise((resolve) => setTimeout(resolve, 25));
    }
    return request;
});

export default axiosConfig;