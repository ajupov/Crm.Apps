import React, { Component } from 'react';
import PropTypes from 'prop-types';
import styles from './Card.less';

class Card extends Component {
  render() {
    return <div className={this.getClassName()}>{this.props.children}</div>;
  }

  getClassName() {
    return `${styles.card} ${this.props.className}`;
  }
}

Card.propTypes = {
  className: PropTypes.string
};

Card.defaultProps = {
  className: ''
};

export default Card;
