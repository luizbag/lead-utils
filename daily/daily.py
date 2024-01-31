import argparse
import random
from datetime import date
import configparser

CONFIG_FILE = "config.ini"

def setup_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(
        prog='Random Daily',
        description='Generates random list for daily meetings')
    parser.add_argument('-c', dest='config_file', help='Config file location')
    parser.add_argument('-t', dest='team', help='Team Choice')
    return parser

def setup_config(config_file_name) -> configparser.ConfigParser:
    parser = configparser.ConfigParser()
    parser.read(config_file_name)
    return parser

def write_notes(config, team, missing, notes) -> None:
    with open(config['file_path'], config['write_mode'], encoding="utf-8") as f:
        f.write(str.format('\nDaily for team {} on {}\n', team.capitalize(), date.today().isoformat()))
        for k, v in notes.items():
            f.write(str.format('- {}: {}\n', k, v))
        if missing:
            f.write(str.format("Missing: {}\n", ", ".join(missing)))

def main(config, team=''):
    notes = {}
    config_section = config["config"]
    teams = config_section["teams"].split()
    if not team:
        team = input(str.format("Choose a team ({}): ", ", ".join(teams)))
    daily = config_section[team].split()
    random.shuffle(daily)
    missing = []
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
    notes_config = config["notes"]
    write_notes(notes_config, team, missing, notes)

if __name__ == "__main__":
    notes = {}
    try:
        parser = setup_parser()
        args = parser.parse_args()
        team = ''
        if args.team:
            team = args.team
        config_file_name = args.config_file or CONFIG_FILE
        config = setup_config(config_file_name)
        main(config, team)
    except KeyboardInterrupt:
        pass
    finally:
        print('Bye')
