import http from 'k6/http';
import { sleep } from 'k6';
import {config} from './config.js';


export const options = {
    stages: [
        { duration: '5s', target: 5},
        { duration: '10s', target: 5},
        { duration: '30s', target: 20},
        { duration: '5s', target: 20},
        { duration: '30s', target: 5},
        { duration: '5s', target: 20},
        { duration: '30s', target: 5},
        { duration: '5s', target: 5}
    ],

    thresholds: {
        http_req_duration: ['p(95)<200']
    }
}

export default function () {
  http.get(config.API_ROLE,
    {
        headers: {
            accept: 'application/json',
            authorization: config.TOKEN
        }
    }
    );
  sleep(1);
}