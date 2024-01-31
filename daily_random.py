import argparse
import random

teams = {
    'frodo': [
        'Lorena',
        'Alejandro',
        'Josué',
        'Swapnil',
        'SaiCharan',
        'Paulo'],
    'aragorn': [
        'Marcel',
        'Tiago',
        'Ricardo',
        'Andreia',
        'Ramendra',
        'Richa',
        'Md'],
    'global_apps': [
        'Cláudio',
        'Hugo',
        'Matheus',
        'José',
        'Felipe',
        'Adhan',
        'Tiago'],
    'test': [
        'Alone',
        'Sad']}

def setup_parser():
    parser = argparse.ArgumentParser(
        prog='Random Daily',
        description='Generates random list for daily meetings')
    parser.add_argument('-c', dest='config_file', help='Config file location')
    return parser

if __name__ == "__main__":
    notes = {}
    try:
        parser = setup_parser()
        args = parser.parse_args()
        config_file_name = args.config_file or "config.yaml"
        daily = teams[args.team].copy()
        random.shuffle(daily)
        missing = []
        print(daily)
        for element in daily:
            print(element)
            present = input('Present (Y/n):')
            if(present.casefold() != 'n'):
                feedback = input('Feedback: ')
                notes[element] = feedback
                continue
            missing.append(element)
        if missing:
            print('Missing people: ', missing)
        else:
            print('Everybody showed up!')
    except:
        pass
    finally:i
        print('Bye')
        for k, v in notes.items():
            print(k, v)
