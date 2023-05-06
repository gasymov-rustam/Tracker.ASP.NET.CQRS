import http from 'k6/http';
import { sleep } from 'k6';
import {config} from './config.js';

export const options = {
    stages: [
        { duration: '30s', target: 16},
        { duration: '1h', target: 16},
        { duration: '10m', target: 5},
        { duration: '5s', target: 0}
    ],

    thresholds: {
        http_req_duration: ['p(95)<1000']
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