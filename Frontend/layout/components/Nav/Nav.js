import React, {Component} from 'react';
import PropTypes from 'prop-types';
import Clock from 'react-live-clock';
import * as moment from 'moment';
import {Path} from '../../../enums/Path';
import Icon from '../../../components/Icon/Icon';
import Link from '../../../components/Link/Link';
import styles from './Nav.less';

const locale = 'ru';
const dateFormat = 'DD MMM';
const timeFormat = 'HH:mm';

class Nav extends Component {
    constructor() {
        super();

        moment.locale(locale);
    }

    render() {
        return (
            <nav className={styles.nav}>
                <div className={styles.left}>
                    <h3>
                        <Icon name='bars'/>
                        <span className={styles.crmNameText}>CRM</span>
                    </h3>
                </div>
                <div className={styles.center}>
                    <div className={styles.dateTime}>
                        <span className={styles.navDate}>
                            <Clock format={dateFormat} ticking={true} timezone={this.props.timeZone}/>
                        </span>
                        <span className={styles.navTime}>
                            <Clock format={timeFormat} ticking={true} timezone={this.props.timeZone}/>
                        </span>
                    </div>
                    <div className={styles.activities}>
                        <Icon name='tasks'/>
                        <span>Задачи на день</span>
                        <span className={styles.activitiesCount}>{this.props.activitiesCount}</span>
                    </div>
                </div>
                <div className={styles.right}>
                    <Link className={styles.name} value={Path.account}>{this.props.userName}</Link>
                    <img className={styles.avatar} src={this.props.userAvatarUrl}/>
                </div>
            </nav>
        );
    }
}

Nav.propTypes = {
    userName: PropTypes.string.isRequired,
    userAvatarUrl: PropTypes.string.isRequired,
    activitiesCount: PropTypes.number.isRequired,
    timeZone: PropTypes.string.isRequired
};

export default Nav;