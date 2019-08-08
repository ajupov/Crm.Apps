import React, {Component} from 'react';
import PropTypes from 'prop-types';

class Icon extends Component {
    render() {
        return <i className={this.getClassName()}/>;
    }

    getClassName() {
        return `fa fa-${this.props.name} ${this.props.className}`;
    }
}

Icon.propTypes = {
    className: PropTypes.string,
    name: PropTypes.string.isRequired
};

Icon.defaultProps = {
    className: ''
};

export default Icon;
