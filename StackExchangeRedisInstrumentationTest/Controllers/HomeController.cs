using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace StackExchangeRedisInstrumentationTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDatabase _database;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public HomeController(IDatabase database, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _database = database;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        public IActionResult Index() => View(_actionDescriptorCollectionProvider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>());

        public async Task<IActionResult> DebugObjectAsync()
        {
            await _database.StringSetAsync("test_key_debug", "hello world");
            await _database.DebugObjectAsync("test_key_debug");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ExecuteAsync()
        {
            await _database.ExecuteAsync("MGET", "key1", "key2");
            await _database.ExecuteAsync("MGET", new Collection<object> { "key1", "key2" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GeoAddAsync()
        {
            await _database.GeoAddAsync("geo_key_1", 0, 0, "Secret Base");
            await _database.GeoAddAsync("geo_key_2", new GeoEntry(0, 0, "Secret Base"));
            await _database.GeoAddAsync("geo_key_3", new[] { new GeoEntry(1, 0, "Other Base") });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GeoDistanceAsync()
        {
            await _database.GeoAddAsync("places", new[] { new GeoEntry(-82.476389, 27.968056, "Tampa"), new GeoEntry(-122.416667, 37.783333, "San Francisco") });

            await _database.GeoDistanceAsync("places", "Tampa", "San Francisco", GeoUnit.Miles);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GeoHashAsync()
        {
            await _database.GeoAddAsync("places", new[] { new GeoEntry(-82.476389, 27.968056, "Tampa"), new GeoEntry(-122.416667, 37.783333, "San Francisco") });

            await _database.GeoHashAsync("places", "Tampa");
            await _database.GeoHashAsync("places", new RedisValue[] { "Tampa", "San Francisco" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GeoPositionAsync()
        {
            await _database.GeoAddAsync("places", new[] { new GeoEntry(-82.476389, 27.968056, "Tampa"), new GeoEntry(-122.416667, 37.783333, "San Francisco") });

            await _database.GeoPositionAsync("places", "Tampa");
            await _database.GeoPositionAsync("places", new RedisValue[] { "Tampa", "San Francisco" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GeoRadiusAsync()
        {
            await _database.GeoAddAsync("places", new[] { new GeoEntry(-82.476389, 27.968056, "Tampa"), new GeoEntry(-122.416667, 37.783333, "San Francisco") });

            await _database.GeoRadiusAsync("places", "Tampa", 50);
            await _database.GeoRadiusAsync("places", 0, 0, 200);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GeoRemoveAsync()
        {
            await _database.GeoAddAsync("places_to_remove", new[] { new GeoEntry(-82.476389, 27.968056, "Tampa"), new GeoEntry(-122.416667, 37.783333, "San Francisco") });

            await _database.GeoRemoveAsync("places_to_remove", "Tampa");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> HashDecrementAsync()
        {
            await _database.HashSetAsync("hash_all_day", "number", 1);
            await _database.HashDecrementAsync("hash_all_day", "number", 1);
            await _database.HashDecrementAsync("hash_all_day", "number");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> HashDeleteAsync()
        {
            await _database.HashSetAsync("hash_all_day", "number", 1);
            await _database.HashSetAsync("hash_all_day", "letter", "o");

            await _database.HashDeleteAsync("hash_all_day", "number");
            await _database.HashDeleteAsync("hash_all_day", new RedisValue[] { "number", "letter" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> HashExistsAsync()
        {
            await _database.HashSetAsync("hashes", "what", "ever");

            await _database.HashExistsAsync("hashes", "what");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> HashGetAllAsync()
        {
            await _database.HashSetAsync("hashes", "what", "ever");

            await _database.HashGetAllAsync("hashes");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> HashGetAsync()
        {
            await _database.HashSetAsync("hashes", "what", "ever");

            await _database.HashGetAsync("hashes", "what");
            await _database.HashGetAsync("hashes", new RedisValue[] { "what", "where" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> HashGetLeaseAsync()
        {
            await _database.HashSetAsync("hashes", "what", "ever");

            await _database.HashGetLeaseAsync("hashes", "what");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> HashIncrementAsync()
        {
            await _database.HashSetAsync("hash_all_day", "number", 1);

            await _database.HashIncrementAsync("hash_all_day", "number", 1);
            await _database.HashIncrementAsync("hash_all_day", "number");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> HashKeysAsync()
        {
            await _database.HashSetAsync("hash_all_day", "number", 1);

            await _database.HashKeysAsync("hash_all_day");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> HashLengthAsync()
        {
            await _database.HashSetAsync("hash_all_day", "number", 1);

            await _database.HashLengthAsync("hash_all_day");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> HashSetAsync()
        {
            await _database.HashSetAsync("hash_all_day", "number", 1);
            await _database.HashSetAsync("hash_add_day", new[] { new HashEntry("holy", "toledo") });

            await _database.HashLengthAsync("hash_all_day");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> HashValuesAsync()
        {
            await _database.HashSetAsync("hash_all_day", "number", 1);

            await _database.HashValuesAsync("hash_all_day");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> HyperLogLogAddAsync()
        {
            await _database.HyperLogLogAddAsync("hyper", "hello world");
            await _database.HyperLogLogAddAsync("hyper_2", new RedisValue[] { "hello", "world" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> HyperLogLogLengthAsync()
        {
            await _database.HyperLogLogLengthAsync("hyper");
            await _database.HyperLogLogLengthAsync(new RedisKey[] { "hyper", "hyper_2" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> HyperLogLogMergeAsync()
        {
            await _database.HyperLogLogMergeAsync("dest", new RedisKey[] { "hyper_1", "hyper_2" });
            await _database.HyperLogLogMergeAsync("dest", "first_hyper", "second_hyper");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> IdentifyEndpointAsync()
        {
            await _database.IdentifyEndpointAsync("wat");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> KeyDeleteAsync()
        {
            await _database.KeyDeleteAsync("wat");
            await _database.KeyDeleteAsync(new RedisKey[] { "key_1", "key_2" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> KeyDumpAsync()
        {
            await _database.KeyDumpAsync("wat");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> KeyExistsAsync()
        {
            await _database.KeyExistsAsync("hey now");
            await _database.KeyExistsAsync(new RedisKey[] { "hello", "world" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> KeyExpireAsync()
        {
            await _database.KeyExpireAsync("gone", DateTime.UtcNow.AddHours(1));
            await _database.KeyExpireAsync("finished", TimeSpan.FromHours(400));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> KeyIdleTimeAsync()
        {
            await _database.KeyIdleTimeAsync("whatever");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> KeyMoveAsync()
        {
            await _database.KeyMoveAsync("hrmmm", 10);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> KeyPersistAsync()
        {
            await _database.KeyPersistAsync("blah");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> KeyRandomAsync()
        {
            await _database.KeyRandomAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> KeyRenameAsync()
        {
            await _database.StringSetAsync("old_key", "whatever");
            await _database.KeyRenameAsync("old_key", "new_key");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> KeyTimeToLiveAsync()
        {
            await _database.KeyTimeToLiveAsync("old_key");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> KeyTypeAsync()
        {
            await _database.KeyTypeAsync("old_key");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListGetByIndexAsync()
        {
            await _database.ListGetByIndexAsync("list", 0);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListInsertAfterAsync()
        {
            await _database.ListInsertAfterAsync("list", "asdf", "jkl;");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListInsertBeforeAsync()
        {
            await _database.ListInsertBeforeAsync("list", "what", "ever");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListLeftPopAsync()
        {
            await _database.ListLeftPopAsync("list_pop_async");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListLeftPushAsync()
        {
            await _database.ListLeftPushAsync("list_pop_async", "wat");
            await _database.ListLeftPushAsync("list_pop_async", new RedisValue[] { "hello", "world" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListLengthAsync()
        {
            await _database.ListLengthAsync("list_pop_async");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListRangeAsync()
        {
            await _database.ListRangeAsync("listssssss");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListRemoveAsync()
        {
            await _database.ListRemoveAsync("removing", "whatever");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListRightPopAsync()
        {
            await _database.ListRightPopAsync("whoa");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListRightPopLeftPushAsync()
        {
            await _database.ListRightPopLeftPushAsync("pop_then_push", "dest_push");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListRightPushAsync()
        {
            await _database.ListRightPushAsync("list_push_1", new RedisValue[] { "val1", "val2" });
            await _database.ListRightPushAsync("list_push_2", "value");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListSetByIndexAsync()
        {
            await _database.ListLeftPushAsync("list_set_index", "hi");
            await _database.ListSetByIndexAsync("list_set_index", 0, "asdf");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListTrimAsync()
        {
            await _database.ListTrimAsync("list_trim", 0, 0);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> LockExtendAsync()
        {
            await _database.LockExtendAsync("lock", "lock_value", TimeSpan.FromSeconds(1));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> LockQueryAsync()
        {
            await _database.LockQueryAsync("lock");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> LockReleaseAsync()
        {
            await _database.LockReleaseAsync("lock", "asdf");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> LockTakeAsync()
        {
            await _database.LockTakeAsync("lock", "asdf", TimeSpan.FromMilliseconds(1));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> PublishAsync()
        {
            await _database.PublishAsync("test_channel", "woot");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ScriptEvaluateAsync()
        {
            // Has 4 overloads...
            await _database.ScriptEvaluateAsync("return 0");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SetAddAsync()
        {
            await _database.SetAddAsync("set_add", "hello");
            await _database.SetAddAsync("set_add", new RedisValue[] { "Hello", "World" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SetCombineAndStoreAsync()
        {
            await _database.SetCombineAndStoreAsync(SetOperation.Difference, "whatever", "first", "second");
            await _database.SetCombineAndStoreAsync(SetOperation.Union, "whatever_2", new RedisKey[] { "first", "second" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SetCombineAsync()
        {
            await _database.SetCombineAsync(SetOperation.Difference, "whatever", "first");
            await _database.SetCombineAsync(SetOperation.Union, new RedisKey[] { "first", "second" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SetContainsAsync()
        {
            await _database.SetContainsAsync("test_set", "whatever");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SetLengthAsync()
        {
            await _database.SetLengthAsync("test_set");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SetMembersAsync()
        {
            await _database.SetMembersAsync("test_set");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SetMoveAsync()
        {
            await _database.SetMoveAsync("set_source", "set_dest", "whatever");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SetPopAsync()
        {
            await _database.SetPopAsync("set_to_pop_from");
            await _database.SetPopAsync("set_to_pop_from", 123);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SetRandomMemberAsync()
        {
            await _database.SetRandomMemberAsync("set_to_pop_from");
            await _database.SetRandomMembersAsync("set_to_pop_from", 123);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SetRemoveAsync()
        {
            await _database.SetRemoveAsync("set_to_pop_from", "blah");
            await _database.SetRemoveAsync("set_to_pop_from", new RedisValue[] { "key1", "key2" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortAndStoreAsync()
        {
            await _database.SortAndStoreAsync("set_to_pop_from", "blah");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortAsync()
        {
            await _database.SortAsync("set_to_pop_from");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetAddAsync()
        {
            await _database.SortedSetAddAsync("sorted", new SortedSetEntry[] { new SortedSetEntry("woo", 2) });
            await _database.SortedSetAddAsync("sorted", "hoo", 2);
            await _database.SortedSetAddAsync("sorted", new SortedSetEntry[] { new SortedSetEntry("woo", 2) }, When.Always);
            await _database.SortedSetAddAsync("sorted", "hoo", 2, When.Exists);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetCombineAndStoreAsync()
        {
            await _database.SortedSetCombineAndStoreAsync(SetOperation.Union, "dest", "first", "second");
            await _database.SortedSetCombineAndStoreAsync(SetOperation.Union, "dest", new RedisKey[] { "wat", "hoo" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetDecrementAsync()
        {
            await _database.SortedSetDecrementAsync("sorted_set", "meh", 1);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetIncrementAsync()
        {
            await _database.SortedSetIncrementAsync("sorted_set", "meh", 1);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetLengthAsync()
        {
            await _database.SortedSetLengthAsync("whatever");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetLengthByValueAsync()
        {
            await _database.SortedSetLengthByValueAsync("whatever", 0, 10);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetPopAsync()
        {
            await _database.SortedSetPopAsync("whatever");
            await _database.SortedSetPopAsync("whatever", 1L);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetRangeByRankAsync()
        {
            await _database.SortedSetRangeByRankAsync("whatever");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetRangeByRankWithScoresAsync()
        {
            await _database.SortedSetRangeByRankWithScoresAsync("whatever");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetRangeByScoreAsync()
        {
            await _database.SortedSetRangeByScoreAsync("whatever");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetRangeByScoreWithScoresAsync()
        {
            await _database.SortedSetRangeByScoreWithScoresAsync("whatever");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetRangeByValueAsync()
        {
            await _database.SortedSetRangeByValueAsync("whatever");
            await _database.SortedSetRangeByValueAsync("whatever", 0, 100, Exclude.Both, 1);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetRankAsync()
        {
            await _database.SortedSetRankAsync("whatever_sorted_set", "wat");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetRemoveAsync()
        {
            await _database.SortedSetRemoveAsync("sorted_set_remove", "asdf");
            await _database.SortedSetRemoveAsync("sorted_set_remove", new RedisValue[] { "thing1", "thing2" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetRemoveRangeByRankAsync()
        {
            await _database.SortedSetRemoveRangeByRankAsync("sorted_set_remove", 0, 1);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetRemoveRangeByScoreAsync()
        {
            await _database.SortedSetRemoveRangeByScoreAsync("sorted_set_remove", 0, 1);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetRemoveRangeByValueAsync()
        {
            await _database.SortedSetRemoveRangeByValueAsync("sorted_set_remove", 0, 1);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SortedSetScoreAsync()
        {
            await _database.SortedSetScoreAsync("sorted_set", "woohoo");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringAppendAsync()
        {
            await _database.StringAppendAsync("what", "ever");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringBitCountAsync()
        {
            await _database.StringBitCountAsync("what_string");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringBitOperationAsync()
        {
            await _database.StringBitOperationAsync(Bitwise.And, "dest_bit_ip", new RedisKey[] { "string_key_1" });
            await _database.StringBitOperationAsync(Bitwise.And, "dest_bit_ip", "string_key_1");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringBitPositionAsync()
        {
            await _database.StringBitPositionAsync("whatever", true);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringDecrementAsync()
        {
            await _database.StringDecrementAsync("string_val_decr", 1);
            await _database.StringDecrementAsync("string_value_decr");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringGetAsync()
        {
            await _database.StringGetAsync("string_val");
            await _database.StringGetAsync(new RedisKey[] { "string_val", "string_val_2" });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringGetBitAsync()
        {
            await _database.StringGetBitAsync("string_val", 1);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringGetLeaseAsync()
        {
            await _database.StringGetLeaseAsync("string_val");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringGetRangeAsync()
        {
            await _database.StringGetRangeAsync("string_val", 0, 10);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringGetSetAsync()
        {
            await _database.StringGetSetAsync("string_val", "hello");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringGetWithExpiryAsync()
        {
            await _database.StringGetWithExpiryAsync("string_val");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringIncrementAsync()
        {
            await _database.StringIncrementAsync("string_val_incr");
            await _database.StringIncrementAsync("string_val_incr", 1);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringLengthAsync()
        {
            await _database.StringLengthAsync("string_val");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringSetAsync()
        {
            await _database.StringSetAsync("string_val", "whatever");
            await _database.StringSetAsync(new[] { new KeyValuePair<RedisKey, RedisValue>("hello_string", "world_string") });

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringSetBitAsync()
        {
            await _database.StringSetBitAsync("string_val", 0, true);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> StringSetRangeAsync()
        {
            await _database.StringSetRangeAsync("string_val", 0, "wooooohooo");

            return RedirectToAction("Index");
        }
    }
}