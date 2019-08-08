import React, {Component} from 'react';
import {NavLink} from 'react-router-dom';
import PropTypes from 'prop-types';
import Icon from './../Icon/Icon';
import styles from './Button.less';

class Button extends Component {
    render() {
        if (this.props.link) {
            return (
                <NavLink className={this.getClassName()} to={this.props.link} disabled={this.props.disabled}>
                    {this.renderIcon()}
                    {this.props.children}
                </NavLink>
            );
        }

        return (
            <a className={this.getClassName()} disabled={this.props.disabled} onClick={this.props.onClick}>
                {this.renderIcon()}
                {this.props.children}
            </a>
        );
    }

    getClassName() {
        let value = styles.button;

        switch (this.props.geometry) {
            case 'circle':
                value += ` ${styles.circle}`;
                break;

            case 'square':
            default:
                value += ` ${styles.square}`;
                break;
        }

        switch (this.props.type) {
            case 'blank':
                value += ` ${styles.blank}`;
                break;

            case 'outlined':
                value += ` ${styles.outlined}`;
                break;

            case 'filled':
            default:
                value += ` ${styles.filled}`;
                break;
        }

        if (this.props.disabled) {
            value += ` ${styles.disabled}`;
        }

        return `${value} ${this.props.className}`;
    }

    renderIcon() {
        if (!this.props.icon) {
            return null;
        }

        return (
            <Icon name={this.props.icon} className={styles.buttonIcon}/>
        );
    }
}

Button.propTypes = {
    className: PropTypes.string,
    disabled: PropTypes.bool,
    link: PropTypes.string,
    icon: PropTypes.string,
    onClick: PropTypes.func,
    geometry: PropTypes.oneOf(['square', 'circle']),
    type: PropTypes.oneOf(['blank', 'outlined', 'filled'])
};

Button.defaultProps = {
    className: '',
    disabled: false,
    geometry: 'square',
    type: 'filled'
};

export default Button;
