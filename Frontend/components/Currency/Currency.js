import React, {Component} from 'react';
import {NavLink} from 'react-router-dom';
import PropTypes from 'prop-types';
import Link from '../Link/Link';

class Currency extends Component {
    render() {
        const value = this.props.value.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1 ');

        return (
            <span className={this.getClassName()}>{value} ₽</span>
        );
    }

    getClassName() {
        return this.props.className;
    }
}

Currency.propTypes = {
    className: PropTypes.string,
    value: PropTypes.number.isRequired
};

Currency.defaultProps = {
    className: ''
};

export default Currency;