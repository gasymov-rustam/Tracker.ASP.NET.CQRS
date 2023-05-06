import http from 'k6/http';
import { sleep } from 'k6';
import {config} from './config.js';

export const options = {
    vus: 3,
    duration: '30s',

    thresholds: {
        http_req_duration: ['p(95)<1000']
    }
}

export default function () {
  console.log('Running smoke test', config.API_ROLE);
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