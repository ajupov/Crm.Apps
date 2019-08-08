import React, {Component} from 'react';
import PropTypes from 'prop-types';
import styles from './Chip.less';

class Chip extends Component {
    render() {
        return <div className={this.getClassName()}>{this.props.children}</div>;
    }

    getClassName() {
        return `${styles.chip} ${this.props.className}`;
    }
}

Chip.propTypes = {
    className: PropTypes.string
};

Chip.defaultProps = {
    className: ''
};

export default Chip;
