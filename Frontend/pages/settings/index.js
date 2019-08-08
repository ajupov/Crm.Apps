import * as React from 'react';
import {Route} from 'react-router-dom';
import {NavLink} from 'react-router-dom';
import {Path} from '../../enums/Path';
import {SettingsCommon} from './common/index';
import {Products} from '../products/index';
import './index.less';

export class Settings extends React.Component {
    render() {
        return (
            <div className='settings-layout'>
                <div className='settings-menu'>
                    <h3>Настройки</h3>
                    <ul>
                        <li>
                            <NavLink to={Path.settingsCommon} activeClassName='active'>Основные настройки</NavLink>
                        </li>
                        <li>
                            <NavLink to={Path.products} activeClassName='active'>Настройки продуктов</NavLink>
                        </li>
                    </ul>
                </div>
                <div className='settings-main'>
                    <Route path={Path.settingsCommon} component={SettingsCommon}/>
                </div>
            </div>
        );
    }
}