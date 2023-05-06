import http from 'k6/http';
import { sleep } from 'k6';
import {config} from './config.js';

export const options = {
  stages: [
    { duration: '5s', target: 5 },
    { duration: '30s', target: 10 },
    { duration: '5s', target: 10 },
    { duration: '30s', target: 100 },
    { duration: '5s', target: 200 },
    { duration: '30s', target: 300 },
    { duration: '5s', target: 10 },
  ],

  thresholds: {
    http_req_duration: ['p(95)<1000'],
  },
};

export default function () {
  http.get(config.API_ROLE);
  sleep(1);
}
