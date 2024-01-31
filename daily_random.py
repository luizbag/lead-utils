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
    return parser

def setup_config(config_file_name) -> configparser.ConfigParser:
    parser = configparser.ConfigParser()
    parser.read(config_file_name)
    return parser

if __name__ == "__main__":
    notes = {}
    try:
        parser = setup_parser()
        args = parser.parse_args()
        config_file_name = args.config_file or CONFIG_FILE
        config = setup_config(config_file_name)
        config_section = config["config"]
        teams = config_section["teams"].split()
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
        with open(notes_config['file_path'], notes_config['write_mode'], encoding="utf-8") as f:
            f.write(str.format('\nDaily for team {} on {}\n', team.capitalize(), date.today().isoformat()))
            for k, v in notes.items():
                f.write(str.format('- {}: {}\n', k, v))
            if missing:
                f.write(str.format("Missing: {}", ", ".join(missing)))
    except KeyboardInterrupt:
        pass
    finally:
        print('Bye')
