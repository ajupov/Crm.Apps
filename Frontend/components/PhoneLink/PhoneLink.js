import React, {Component} from 'react';
import {NavLink} from 'react-router-dom';
import PropTypes from 'prop-types';
import Link from '../Link/Link';

class PhoneLink extends Component {
    render() {
        let value = `+7${this.props.value}`;


        return (
            <Link className={this.getClassName()} disabled={this.props.disabled} force={true} target='self'
                  value={`tel:${value}`}>{value}</Link>
        );
    }

    getClassName() {
        return this.props.className;
    }
}

PhoneLink.propTypes = {
    className: PropTypes.string,
    value: PropTypes.string,
    disabled: PropTypes.bool
};

PhoneLink.defaultProps = {
    className: '',
    disabled: false,
    value: ''
};

export default PhoneLink;