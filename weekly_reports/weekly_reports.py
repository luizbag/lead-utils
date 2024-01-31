import win32com.client
import argparse
import configparser

from datetime import datetime, timedelta

CONFIG_FILE = "config.ini"
N_DAYS = 3
WEEKDAY = 0

def setup_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(
        prog = 'Weekly Reports Checker',
        description='Checks if weekly reports were received and sends reminders')
    parser.add_argument('-c', dest='config_file', help='Config file location')
    parser.add_argument('-d', dest="n_days", help="Number of previous days to check for reports")
    parser.add_argument('-w', dest="weekday", help="The day of the week to run the check")
    return parser

def setup_config(config_file_name) -> configparser.ConfigParser:
    parser = configparser.ConfigParser()
    parser.read(config_file_name)
    return parser

def who_sent_report(messages):
    senders = set()
    for message in messages:
        senders.add(message.Sender.Name)
    return senders

def who_is_missing(senders, team):
    missing = set()
    for member in team:
        if(member not in senders):
            missing.add(member)
    return missing

def send_reminders(missing):
    mapi = get_outlook_api()

def get_outlook_api():
    outlook = win32com.client.Dispatch('outlook.application')
    mapi = outlook.GetNamespace("MAPI")
    return mapi

def get_messages(account_folder, messages_folder, n_days):
    mapi = get_outlook_api()

    folder = mapi.Folders[account_folder].Folders[messages_folder]

    messages = folder.items
    now = datetime.utcnow() - timedelta(days=n_days)
    received_dt = now.strftime("%m/%d/%Y")
    messages = messages.Restrict("[ReceivedTime] >= '" + received_dt + "'")
    return messages

def main(config, n_days, weekday):
    if(datetime.now().weekday() != weekday):
        print("Not today")
        return
    
    config_section = config["config"]
    team = config_section["team"].split(',')
    messages = get_messages(config_section["account_folder"], config_section["messages_folder"], n_days)
    senders = who_sent_report(list(messages))
    missing = who_is_missing(senders, team)
    if missing:
        print("Missing:")
    for member in missing:
        print(member)
    send_reminders = input('Send reminders (Y/n): ')
    if(send_reminders.casefold() != 'n'):
        print('send reminders')

if __name__ == "__main__":
    try:
        parser = setup_parser()
        args = parser.parse_args()
        n_days = args.n_days or N_DAYS
        config_file_name = args.config_file or CONFIG_FILE
        weekday = args.weekday or WEEKDAY
        config = setup_config(config_file_name)
        main(config, int(n_days), int(weekday))
    except KeyboardInterrupt:
        pass
    finally:
        print('Bye')

