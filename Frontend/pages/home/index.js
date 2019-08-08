import React, {Component} from 'react';
import {Path} from '../../enums/Path';
import Button from '../../components/Button/Button';
import styles from './index.less';

export class Home extends Component {
    componentDidMount() {
        document.title = 'CRM';
    }

    render() {
        return (
            <div className={styles.layout}>
                <div className={styles.main}></div>
                <div className={styles.actions}>
                    <Button type='outlined' icon='sign-in' link={Path.authentication}>Вход</Button>
                </div>
            </div>
        );
    }
}