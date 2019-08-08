import React, {Component} from 'react';
import {Route} from 'react-router-dom';
import {Path} from '../../../enums/Path';
import Button from '../../../components/Button/Button';
import {Account} from '../../../pages/account';
import {Activities} from '../../../pages/activities';
import {Calendar} from '../../../pages/calendar';
import {Contacts} from '../../../pages/contacts';
import {Dashboard} from '../../../pages/dashboard';
import {Deals} from '../../../pages/deals';
import {Employees} from '../../../pages/employees';
import {Leads} from '../../../pages/leads';
import {Mail} from '../../../pages/mail';
import {Products} from '../../../pages/products';
import LeftMenuLink from '../LeftMenuLink/LeftMenuLink';
import RightMenuLink from '../RightMenuLink/RightMenuLink';
import TopMenuLink from '../TopMenuLink/TopMenuLink';
import styles from './Main.less';

class Main extends Component {
    render() {
        return (
            <main className={styles.main}>
                {this.renderLeft()}
                {this.renderContent()}
                {this.renderRight()}
            </main>
        );
    }

    renderLeft() {
        return (
            <div className={styles.left}>
                <LeftMenuLink icon='home' value={Path.dashboard} text='CRM'/>
                <LeftMenuLink icon='envelope' value={Path.mail} text='Почта'/>
                <LeftMenuLink icon='users' value={Path.employees} text='Сотрудники'/>
                <LeftMenuLink icon='calendar-check-o' value={Path.calendar} text='Календарь'/>
                <LeftMenuLink icon='cog' value={Path.settings} text='Настройки'/>
                <LeftMenuLink icon='puzzle-piece' value='' text='Еще ▼'/>
                <LeftMenuLink icon='cogs' value={Path.settingsMenu} text='настройки меню'/>
            </div>
        );
    }

    renderContent() {
        return (
            <div className={styles.content}>
                <div className={styles.top}>
                    <TopMenuLink value={Path.dashboard} text='Рабочий стол'/>
                    <TopMenuLink value={Path.leads} text='Лиды'/>
                    <TopMenuLink value={Path.deals} text='Сделки'/>
                    <TopMenuLink value={Path.contacts} text='Контакты'/>
                    <TopMenuLink value={Path.products} text='Продукты'/>
                    <TopMenuLink value={Path.activities} text='Активности'/>
                    <TopMenuLink value='' text='Еще ▼'/>
                </div>

                <Route path={Path.account} component={Account}/>
                <Route path={Path.home} exact component={Dashboard}/>
                <Route path={Path.leads} component={Leads}/>
                <Route path={Path.employees} component={Employees}/>
                <Route path={Path.calendar} component={Calendar}/>
                <Route path={Path.deals} component={Deals}/>
                <Route path={Path.contacts} component={Contacts}/>
                <Route path={Path.products} component={Products}/>
                <Route path={Path.activities} component={Activities}/>
                <Route path={Path.mail} component={Mail}/>
            </div>
        );
    }

    renderRight() {
        return (
            <div className={styles.right}>
                <div className={styles.rightTop}>
                    <RightMenuLink icon='question-circle' onClick={() => alert('Справка')}/>
                    <RightMenuLink icon='bell' onClick={() => alert('Уведомления')}/>
                    <RightMenuLink icon='comments' onClick={() => alert('Сообщения')}/>
                    <RightMenuLink icon='star' onClick={() => alert('Избранное')}/>
                    <RightMenuLink icon='fire' onClick={() => alert('Неотложное')}/>
                </div>
                <div className={styles.rightDown}>
                    <Button geometry='circle' type='blank' icon='phone' className={styles.rightMenuDownButton}
                            onClick={() => alert('Телефония')}>
                    </Button>
                </div>
            </div>
        );
    }
}

export default Main;