
import re

class UnknownTokenException(Exception):
    pass

class IllegalArgumentError(ValueError):
    pass

class Token:
    """
    Token Implementation class, which converts string to tokens, using lexer
    """
    def __init__(self, name, value, offset, count):
        self.name = name
        self.value = value
        self.offset = offset
        self.position = count

    def get_position(self):
        return self.position

    def get_offset(self):
        return self.offset

    def get_name(self):
        return self.name

    def get_value(self):
        return self.value

    def is_token(self, token):
        if isinstance(token, self):
            return self.name == token.get_name()
        elif type(token) == str:
            return self.name == token
        else:
            raise IllegalArgumentError('Expected string or Token')

class TokenDefination:

    def __init__(self, name, regex, modifiers='i'):
        self.name = name
        delimiter = self.find_delimeter(regex)

        self.regex = '%s^%s%s%s' % (delimiter, regex, delimiter, modifiers)
        if not self.regex:
            raise IllegalArgumentError('Invalid regex for token %s : %s' % (name, regex))


    def get_regex(self):
        return self.regex

    def get_name(self):
        return self.name

    def find_delimeter(self, regex):
        choices = ['/', '|', '#', '~', '@']
        for choice in choices:
            if choice not in regex:
                return choice

        raise IllegalArgumentError('Unable to determine delimiter for regex %s' % (regex))

class LexerDictConfig:
    """
    Lexer Configuration using a dictionary
    """

    def __init__(self, token_definations={}):
        self.definations = []

        for k,v in token_definations.items():
            if type(v) == TokenDefination:
                self.add_token_defination(v)
            else:
                self.add_token_defination(TokenDefination(v, k))


    def add_token_defination(self, token_defination):
        self.definations.append(token_defination)

    def get_token_definations(self):
        return self.definations

class PyLexer:

    def __init__(self, config):
        self.config = config

    @staticmethod
    def scan(config, input_string):
        tokens = []
        offset = 0
        position = 0
        matches = None
        config = LexerDictConfig(config)

        while len(input_string):
            any_match = False

            for token_defination in config.get_token_definations():
                matches = re.search(token_defination.get_regex(), input_string, flags=re.IGNORECASE)
                if matches is not None:
                    str_matched = matches.group(0)
                    str_len = len(str_matched)

                    if len(token_defination.get_name()) > 0:
                        tokens.append(Token(token_defination.get_name(), str_matched, offset, position))
                        position += 1

                    input_string = input_string[:str_len]
                    any_match = True
                    offset += str_len
                    break

            if not any_match:
                raise UnknownTokenException('At offset %s: %s' %( offset, input_string[0:16] + '...'))
        return tokens

    def get_input(self):
        return self.input

    def get_position(self):
        return self.position

    def get_look_ahead(self):
        return self.lookahead

    def get_token(self):
        return self.token

    def set_input(self, input_string):
        self.input = input_string
        self.reset()
        self.tokens = PyLexer.scan(self.config, input_string)

    def reset(self):
        self.position = 0
        self.peek = 0
        self.token = None
        self.lookahead = None

    def reset_position(self, position=0):
        self.position = position

    def is_next_token(self, token_name):
        return self.lookahead is not None and self.lookahead.get_name() == token_name

    def is_next_token_any(self, token_names):
        return self.lookahead is not None and self.lookahead.get_name() in token_names

    def move_next(self):
        self.peek = 0
        self.token = self.lookahead

        try:
            self.lookahead = self.tokens[self.position]
            self.position += 1
        except IndexError:
            self.lookahead = None

        return self.lookahead != None

    def skip_until(self, token_name):
        while self.lookahead != None and self.lookahead.get_name() != token_name:
            self.move_next()

    def skip_tokens(self, token_names):
        while self.lookahead != None and self.lookahead.get_name() in token_names:
            self.move_next()

    def peeks(self):
        try:
            if self.tokens[self.position + self.peek]:
                self.peek += 1
                return self.tokens[self.position + self.peek]
        except IndexError:
            return None


    def peek_while_tokens(self, token_names):
        token = self.peeks()
        while token:
            if token.get_name() not in token_names:
                break

        return token

    def glimpse(self):
        peek = self.peeks()
        self.peek = 0
        return peek
