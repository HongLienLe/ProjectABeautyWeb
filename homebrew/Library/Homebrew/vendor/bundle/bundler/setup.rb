require 'rbconfig'
# ruby 1.8.7 doesn't define RUBY_ENGINE
ruby_engine = defined?(RUBY_ENGINE) ? RUBY_ENGINE : 'ruby'
ruby_version = RbConfig::CONFIG["ruby_version"]
path = File.expand_path('..', __FILE__)
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/concurrent-ruby-1.1.6/lib/concurrent-ruby"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/i18n-1.8.2/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/minitest-5.14.0/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/thread_safe-0.3.6/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/tzinfo-1.2.6/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/zeitwerk-2.3.0/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/activesupport-6.0.2.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/ast-2.4.0/lib"
$:.unshift "#{path}/"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/connection_pool-2.2.2/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/extensions/universal-darwin-19/2.6.0/json-2.3.0"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/json-2.3.0/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/docile-1.3.2/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/simplecov-html-0.10.2/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/simplecov-0.16.1/lib"
$:.unshift "#{path}/../../../../../../../../Library/Ruby/Gems/2.6.0/gems/sync-0.5.0/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/tins-1.24.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/term-ansicolor-1.7.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/thor-1.0.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/coveralls-0.8.23/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/diff-lcs-1.3/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/extensions/universal-darwin-19/2.6.0/unf_ext-0.0.7.6"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/unf_ext-0.0.7.6/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/unf-0.1.4/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/domain_name-0.5.20190701/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/extensions/universal-darwin-19/2.6.0/hpricot-0.8.6"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/hpricot-0.8.6/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/http-cookie-1.0.3/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/extensions/universal-darwin-19/2.6.0/jaro_winkler-1.5.4"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/jaro_winkler-1.5.4/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/mime-types-data-3.2019.1009/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/mime-types-3.3.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/net-http-digest_auth-1.4.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/net-http-persistent-3.1.0/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/mini_portile2-2.4.0/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/extensions/universal-darwin-19/2.6.0/nokogiri-1.10.9"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/nokogiri-1.10.9/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/ntlm-http-0.1.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/webrobots-0.1.2/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/mechanize-2.7.6/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/mustache-1.1.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/parallel-1.19.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/parallel_tests-2.31.0/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/parser-2.7.0.4/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/plist-3.5.0/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/rainbow-3.0.0/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/extensions/universal-darwin-19/2.6.0/rdiscount-2.2.0.1"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/rdiscount-2.2.0.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/rexml-3.2.4/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/ronn-0.7.3/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/rspec-support-3.9.2/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/rspec-core-3.9.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/rspec-expectations-3.9.0/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/rspec-mocks-3.9.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/rspec-3.9.0/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/rspec-its-1.3.0/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/rspec-retry-0.6.2/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/rspec-wait-0.0.9/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/ruby-progressbar-1.10.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/unicode-display_width-1.6.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/rubocop-0.80.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/rubocop-performance-1.5.2/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/rubocop-rspec-1.38.1/lib"
$:.unshift "#{path}/../#{ruby_engine}/#{ruby_version}/gems/ruby-macho-2.2.0/lib"
