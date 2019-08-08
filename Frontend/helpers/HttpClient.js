import 'isomorphic-fetch';
import {stringify} from 'query-string';

export function get(url, data = {}) {
    return fetch(url + stringify(data), getParams('get'))
        .then(checkError)
        .then(checkRedirect)
        .then(convertToJson);
}

export function post(url, body = {}) {
    return fetch(url, getParams('post', body))
        .then(checkError)
        .then(checkRedirect)
        .then(convertToJson);
}

function getParams(method, postData = {}) {
    const defaultParams = {
        credentials: 'include',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'cache-control': 'no-cache',
            'pragma': 'no-cache',
        }
    };

    let result = Object.assign({}, defaultParams, {'method': method});

    if (method === 'post') {
        return Object.assign({}, result, {'body': JSON.stringify(postData)});
    }
    return result;
}

function checkError(response) {
    if (!response.ok) {
        throw Error(response.statusText);
    }

    return response;
}

function checkRedirect(response) {
    const location = response.headers.get('location');
    if (location) {
        window.location = location;
    }

    return response;
}

function convertToJson(response) {
    return response.json();

    // return new Promise((resolve, reject) => {
    //     if (response) {
    //         response.json().then(json => {
    //             resolve(json)
    //         }).catch((error) => {
    //             debugger;
    //             reject(null);
    //         })
    //     } else {
    //         debugger;
    //         resolve(null)
    //     }
    // });
}

