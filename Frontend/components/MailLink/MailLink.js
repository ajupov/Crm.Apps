import React, {Component} from 'react';
import {NavLink} from 'react-router-dom';
import PropTypes from 'prop-types';
import Link from '../Link/Link';

class MailLink extends Component {
    render() {
        return (
            <Link className={this.getClassName()} disabled={this.props.disabled} force={true} target='self'
                  value={`mailto:${this.props.value}`}>{this.props.value}</Link>
        );
    }

    getClassName() {
        return this.props.className;
    }
}

MailLink.propTypes = {
    className: PropTypes.string,
    value: PropTypes.string,
    disabled: PropTypes.bool
};

MailLink.defaultProps = {
    className: '',
    disabled: false,
    value: ''
};

export default MailLink;