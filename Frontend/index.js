import React from 'react'
import ReactDOM from 'react-dom';
import {BrowserRouter} from 'react-router-dom';
import {Layout} from './layout';
import 'font-awesome/css/font-awesome.min.css';
import './index.less';

const router = (
    <BrowserRouter>
        <Layout/>
    </BrowserRouter>
);

ReactDOM.render(router, document.getElementById('root-js'));