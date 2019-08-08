import React, {Component} from 'react';
import {NavLink} from 'react-router-dom';
import PropTypes from 'prop-types';
import * as moment from 'moment';
import Link from '../Link/Link';

class Date extends Component {
    constructor() {
        super();

        moment.locale('ru');
    }

    render() {
        const value = moment(this.props.value).format('DD.MM.YYYY HH:mm:ss', { trim: false });

        return (
            <span className={this.getClassName()}>{value}</span>
        );
    }

    getClassName() {
        return this.props.className;
    }
}

Date.propTypes = {
    className: PropTypes.string,
    value: PropTypes.string.isRequired
};

Date.defaultProps = {
    className: ''
};

export default Date;